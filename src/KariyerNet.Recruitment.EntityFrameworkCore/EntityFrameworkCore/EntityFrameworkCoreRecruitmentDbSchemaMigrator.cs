using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using KariyerNet.Recruitment.Data;
using Volo.Abp.DependencyInjection;

namespace KariyerNet.Recruitment.EntityFrameworkCore;

public class EntityFrameworkCoreRecruitmentDbSchemaMigrator
    : IRecruitmentDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreRecruitmentDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the RecruitmentDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<RecruitmentDbContext>()
            .Database
            .MigrateAsync();
    }
}
