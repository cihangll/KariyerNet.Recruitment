using System;
using System.Collections.Generic;
using System.Text;
using KariyerNet.Recruitment.Localization;
using Volo.Abp.Application.Services;

namespace KariyerNet.Recruitment;

/* Inherit your application services from this class.
 */
public abstract class RecruitmentAppService : ApplicationService
{
    protected RecruitmentAppService()
    {
        LocalizationResource = typeof(RecruitmentResource);
    }
}
