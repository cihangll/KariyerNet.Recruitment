using KariyerNet.Recruitment.JobAdverts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.TenantManagement.EntityFrameworkCore;

namespace KariyerNet.Recruitment.EntityFrameworkCore;

[DependsOn(
	typeof(RecruitmentDomainModule),
	typeof(AbpIdentityEntityFrameworkCoreModule),
	typeof(AbpOpenIddictEntityFrameworkCoreModule),
	typeof(AbpPermissionManagementEntityFrameworkCoreModule),
	typeof(AbpSettingManagementEntityFrameworkCoreModule),
	typeof(AbpEntityFrameworkCorePostgreSqlModule),
	typeof(AbpBackgroundJobsEntityFrameworkCoreModule),
	typeof(AbpAuditLoggingEntityFrameworkCoreModule),
	typeof(AbpTenantManagementEntityFrameworkCoreModule),
	typeof(AbpFeatureManagementEntityFrameworkCoreModule)
	)]
public class RecruitmentEntityFrameworkCoreModule : AbpModule
{
	public override void PreConfigureServices(ServiceConfigurationContext context)
	{
		// https://www.npgsql.org/efcore/release-notes/6.0.html#opting-out-of-the-new-timestamp-mapping-logic
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		RecruitmentEfCoreEntityExtensionMappings.Configure();
	}

	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		context.Services.AddAbpDbContext<RecruitmentDbContext>(options =>
		{
			/* Remove "includeAllEntities: true" to create
			 * default repositories only for aggregate roots */
			options.AddDefaultRepositories(includeAllEntities: true);

			options.AddDefaultRepository<JobAdvertBenefit>();
		});

		Configure<AbpDbContextOptions>(options =>
		{
			/* The main point to change your DBMS.
			 * See also RecruitmentMigrationsDbContextFactory for EF Core tooling. */
			options.UseNpgsql();
		});

		Configure<AbpEntityOptions>(options =>
		{
			options.Entity<JobAdvert>(jobAdvertOptions =>
			{
				jobAdvertOptions.DefaultWithDetailsFunc = query => query.Include(x => x.JobAdvertBenefits);
			});
		});
	}
}
