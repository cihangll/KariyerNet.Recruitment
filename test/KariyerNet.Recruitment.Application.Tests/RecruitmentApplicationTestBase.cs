using Volo.Abp.Modularity;

namespace KariyerNet.Recruitment;

public abstract class RecruitmentApplicationTestBase<TStartupModule> : RecruitmentTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
