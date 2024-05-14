using Volo.Abp.Modularity;

namespace KariyerNet.Recruitment;

/* Inherit from this class for your domain layer tests. */
public abstract class RecruitmentDomainTestBase<TStartupModule> : RecruitmentTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
