using KariyerNet.Recruitment.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace KariyerNet.Recruitment.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class RecruitmentController : AbpControllerBase
{
    protected RecruitmentController()
    {
        LocalizationResource = typeof(RecruitmentResource);
    }
}
