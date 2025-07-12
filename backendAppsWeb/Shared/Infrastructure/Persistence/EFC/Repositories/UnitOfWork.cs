using backendAppsWeb.Shared.Domain.Repositories;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration;

namespace backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}