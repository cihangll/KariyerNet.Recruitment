using Volo.Abp.Modularity;

namespace KariyerNet.Recruitment;

[DependsOn(
    typeof(RecruitmentApplicationModule),
    typeof(RecruitmentDomainTestModule)
)]
public class RecruitmentApplicationTestModule : AbpModule
{

}
