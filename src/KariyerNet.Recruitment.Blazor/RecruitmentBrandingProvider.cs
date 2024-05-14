using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace KariyerNet.Recruitment.Blazor;

[Dependency(ReplaceServices = true)]
public class RecruitmentBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Recruitment";
}
