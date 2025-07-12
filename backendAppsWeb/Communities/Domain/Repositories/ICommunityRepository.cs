using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Shared.Domain.Repositories;

namespace backendAppsWeb.Communities.Domain.Repositories;

public interface ICommunityRepository : IBaseRepository<Community>
{
    Task<IEnumerable<Community>> GetCommunitiesByNameAsync(string name);

    Task<Community?> FindDetailedByIdAsync(int id);
    
    Task UpdateAsync(Community community);
}