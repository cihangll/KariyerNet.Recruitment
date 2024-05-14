using Volo.Abp.Features;

namespace KariyerNet.Recruitment.Features;

public class RecruitmentFeatureDefinitionProvider : FeatureDefinitionProvider
{
	public override void Define(IFeatureDefinitionContext context)
	{
		var appGroup = context.AddGroup(RecruitmentFeatures.GroupName);

		appGroup.AddFeature(RecruitmentFeatures.MaxJobAdvertLimit, "2");
	}
}