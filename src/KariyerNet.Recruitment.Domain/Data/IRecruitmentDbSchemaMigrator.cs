using System.Threading.Tasks;

namespace KariyerNet.Recruitment.Data;

public interface IRecruitmentDbSchemaMigrator
{
    Task MigrateAsync();
}
