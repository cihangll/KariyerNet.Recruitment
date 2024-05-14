using Xunit;

namespace KariyerNet.Recruitment.EntityFrameworkCore;

[CollectionDefinition(RecruitmentTestConsts.CollectionDefinitionName)]
public class RecruitmentEntityFrameworkCoreCollection : ICollectionFixture<RecruitmentEntityFrameworkCoreFixture>
{

}
