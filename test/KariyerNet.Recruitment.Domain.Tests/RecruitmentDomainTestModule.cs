using Volo.Abp.Modularity;

namespace KariyerNet.Recruitment;

[DependsOn(
    typeof(RecruitmentDomainModule),
    typeof(RecruitmentTestBaseModule)
)]
public class RecruitmentDomainTestModule : AbpModule
{

}
