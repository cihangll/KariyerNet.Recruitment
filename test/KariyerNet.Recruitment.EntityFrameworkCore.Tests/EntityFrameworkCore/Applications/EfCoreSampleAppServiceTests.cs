using KariyerNet.Recruitment.Samples;
using Xunit;

namespace KariyerNet.Recruitment.EntityFrameworkCore.Applications;

[Collection(RecruitmentTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<RecruitmentEntityFrameworkCoreTestModule>
{

}
