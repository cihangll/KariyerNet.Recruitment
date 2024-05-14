using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace KariyerNet.Recruitment;

[Dependency(ReplaceServices = true)]
public class RecruitmentBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "Recruitment";
}
