using Volo.Abp.Account;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.EventBus.RabbitMq;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SettingManagement;
using Volo.Abp.TenantManagement;

namespace KariyerNet.Recruitment;

[DependsOn(
	typeof(RecruitmentDomainModule),
	typeof(AbpAccountApplicationModule),
	typeof(RecruitmentApplicationContractsModule),
	typeof(AbpIdentityApplicationModule),
	typeof(AbpPermissionManagementApplicationModule),
	typeof(AbpTenantManagementApplicationModule),
	typeof(AbpFeatureManagementApplicationModule),
	typeof(AbpSettingManagementApplicationModule)
	)]
[DependsOn(typeof(AbpEventBusRabbitMqModule))]
public class RecruitmentApplicationModule : AbpModule
{
	public override void ConfigureServices(ServiceConfigurationContext context)
	{
		Configure<AbpAutoMapperOptions>(options =>
		{
			options.AddMaps<RecruitmentApplicationModule>();
		});

		Configure<AbpDistributedEntityEventOptions>(options =>
		{
		});
	}
}
