using Volo.Abp.Settings;

namespace KariyerNet.Recruitment.Settings;

public class RecruitmentSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(RecruitmentSettings.MySetting1));
    }
}
