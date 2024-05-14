using KariyerNet.Recruitment.Localization;
using Volo.Abp.AspNetCore.Components;

namespace KariyerNet.Recruitment.Blazor;

public abstract class RecruitmentComponentBase : AbpComponentBase
{
    protected RecruitmentComponentBase()
    {
        LocalizationResource = typeof(RecruitmentResource);
    }
}
