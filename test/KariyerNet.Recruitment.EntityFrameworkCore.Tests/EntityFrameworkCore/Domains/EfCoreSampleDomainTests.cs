using KariyerNet.Recruitment.Samples;
using Xunit;

namespace KariyerNet.Recruitment.EntityFrameworkCore.Domains;

[Collection(RecruitmentTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<RecruitmentEntityFrameworkCoreTestModule>
{

}
