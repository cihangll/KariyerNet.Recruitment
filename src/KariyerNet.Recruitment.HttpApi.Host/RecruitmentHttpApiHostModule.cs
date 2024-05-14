using KariyerNet.Recruitment.EntityFrameworkCore;
using KariyerNet.Recruitment.MultiTenancy;
using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.MultiTenancy;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.DistributedLocking;
using Volo.Abp.Modularity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Swashbuckle;
using Volo.Abp.VirtualFileSystem;

namespace KariyerNet.Recruitment;

[DependsOn(
	typeof(RecruitmentHttpApiModule),
	typeof(AbpAutofacModule),
	typeof(AbpCachingStackExchangeRedisModule),
	typeof(AbpDistributedLockingModule),
	typeof(AbpAspNetCoreMvcUiMultiTenancyModule),
	typeof(RecruitmentApplicationModule),
	typeof(RecruitmentEntityFrameworkCoreModule),
	typeof(AbpAspNetCoreSerilogModule),
	typeof(AbpSwashbuckleModule)
)]
public class RecruitmentHttpApiHostModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		var configuration = context.Services.GetConfiguration();
		var hostingEnvironment = context.Services.GetHostingEnvironment();

		ConfigureConventionalControllers();
		ConfigureAuthentication(context, configuration);
		ConfigureCache(configuration);
		ConfigureVirtualFileSystem(context);
		ConfigureDataProtection(context, configuration, hostingEnvironment);
		ConfigureDistributedLocking(context, configuration);
		ConfigureCors(context, configuration);
		ConfigureSwaggerServices(context, configuration);

		Configure<AbpExceptionHandlingOptions>(options =>
		{
			options.SendExceptionsDetailsToClients = true;
			options.SendStackTraceToClients = false;
		});
	}

	private void ConfigureCache(IConfiguration configuration)
	{
		Configure<AbpDistributedCacheOptions>(options => { options.KeyPrefix = "Recruitment:"; });
	}

	private void ConfigureVirtualFileSystem(ServiceConfigurationContext context)
	{
		var hostingEnvironment = context.Services.GetHostingEnvironment();

		if (hostingEnvironment.IsDevelopment())
		{
			Configure<AbpVirtualFileSystemOptions>(options =>
			{
				options.FileSets.ReplaceEmbeddedByPhysical<RecruitmentDomainSharedModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						$"..{Path.DirectorySeparatorChar}KariyerNet.Recruitment.Domain.Shared"));
				options.FileSets.ReplaceEmbeddedByPhysical<RecruitmentDomainModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						$"..{Path.DirectorySeparatorChar}KariyerNet.Recruitment.Domain"));
				options.FileSets.ReplaceEmbeddedByPhysical<RecruitmentApplicationContractsModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						$"..{Path.DirectorySeparatorChar}KariyerNet.Recruitment.Application.Contracts"));
				options.FileSets.ReplaceEmbeddedByPhysical<RecruitmentApplicationModule>(
					Path.Combine(hostingEnvironment.ContentRootPath,
						$"..{Path.DirectorySeparatorChar}KariyerNet.Recruitment.Application"));
			});
		}
	}

	private void ConfigureConventionalControllers()
	{
		Configure<AbpAspNetCoreMvcOptions>(options =>
		{
			options.ConventionalControllers.Create(typeof(RecruitmentApplicationModule).Assembly);
		});
	}

	private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
	{
		context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.Authority = configuration["AuthServer:Authority"];
				options.RequireHttpsMetadata = configuration.GetValue<bool>("AuthServer:RequireHttpsMetadata");
				options.Audience = "Recruitment";
			});

		context.Services.Configure<AbpClaimsPrincipalFactoryOptions>(options =>
		{
			options.IsDynamicClaimsEnabled = true;
		});
	}

	private static void ConfigureSwaggerServices(ServiceConfigurationContext context, IConfiguration configuration)
	{
		context.Services.AddAbpSwaggerGenWithOAuth(
			configuration["AuthServer:Authority"]!,
			new Dictionary<string, string>
			{
					{"Recruitment", "Recruitment API"}
			},
			options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo { Title = "Recruitment API", Version = "v1" });
				options.DocInclusionPredicate((docName, description) => true);
				options.CustomSchemaIds(type => type.FullName);

				options.HideAbpEndpoints();
			});
	}

	private void ConfigureDataProtection(
		ServiceConfigurationContext context,
		IConfiguration configuration,
		IWebHostEnvironment hostingEnvironment)
	{
		var dataProtectionBuilder = context.Services.AddDataProtection().SetApplicationName("Recruitment");
		if (!hostingEnvironment.IsDevelopment())
		{
			var redis = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
			dataProtectionBuilder.PersistKeysToStackExchangeRedis(redis, "Recruitment-Protection-Keys");
		}
	}

	private void ConfigureDistributedLocking(
		ServiceConfigurationContext context,
		IConfiguration configuration)
	{
		context.Services.AddSingleton<IDistributedLockProvider>(sp =>
		{
			var connection = ConnectionMultiplexer.Connect(configuration["Redis:Configuration"]!);
			return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
		});
	}

	private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
	{
		context.Services.AddCors(options =>
		{
			options.AddDefaultPolicy(builder =>
			{
				builder
					.WithOrigins(configuration["App:CorsOrigins"]?
						.Split(",", StringSplitOptions.RemoveEmptyEntries)
						.Select(o => o.RemovePostFix("/"))
						.ToArray() ?? Array.Empty<string>())
					.WithAbpExposedHeaders()
					.SetIsOriginAllowedToAllowWildcardSubdomains()
					.AllowAnyHeader()
					.AllowAnyMethod()
					.AllowCredentials();
			});
		});
	}

	public override void OnApplicationInitialization(ApplicationInitializationContext context)
	{
		var app = context.GetApplicationBuilder();
		var env = context.GetEnvironment();

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}

		app.UseAbpRequestLocalization();
		app.UseCorrelationId();
		app.UseStaticFiles();
		app.UseRouting();
		app.UseCors();
		app.UseAuthentication();

		if (MultiTenancyConsts.IsEnabled)
		{
			app.UseMultiTenancy();
		}

		app.UseUnitOfWork();
		app.UseDynamicClaims();
		app.UseAuthorization();

		app.UseSwagger();
		app.UseAbpSwaggerUI(options =>
		{
			options.SwaggerEndpoint("/swagger/v1/swagger.json", "Recruitment API");

			var configuration = context.GetConfiguration();
			options.OAuthClientId(configuration["AuthServer:SwaggerClientId"]);
			options.OAuthScopes("Recruitment");
		});

		app.UseAuditing();
		app.UseAbpSerilogEnrichers();
		app.UseConfiguredEndpoints();
	}
}
