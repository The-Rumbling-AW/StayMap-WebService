using Microsoft.EntityFrameworkCore;
using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Communities.Domain.Repositories;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Repositories;


namespace backendAppsWeb.Communities.Infrastructure.Persistence.EFC.Repositories;

public class CommunityRepository(AppDbContext dbContext)
    : BaseRepository<Community>(dbContext), ICommunityRepository
{
    public async Task<IEnumerable<Community>> GetCommunitiesByNameAsync(string name)
    {
        return await Context.Set<Community>()
            .Where(c => c.Name.ToLower() == name.ToLower())
            .ToListAsync();
    }

    public async Task<Community?> FindDetailedByIdAsync(int id)
    {
        return await Context.Set<Community>()
            .Include(c => c.UserLinks)
            .Include(c => c.Posts)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<IEnumerable<Community>> ListAsync()
    {
        return await Context.Set<Community>()
            .Include(c => c.UserLinks)
            .Include(c => c.Posts)
            .ToListAsync();
    }
    public async Task UpdateAsync(Community community)
    {
        Context.Set<Community>().Update(community);
        await Task.CompletedTask;
    }
}