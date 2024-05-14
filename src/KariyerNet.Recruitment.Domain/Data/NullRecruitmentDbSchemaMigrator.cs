using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace KariyerNet.Recruitment.Data;

/* This is used if database provider does't define
 * IRecruitmentDbSchemaMigrator implementation.
 */
public class NullRecruitmentDbSchemaMigrator : IRecruitmentDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
