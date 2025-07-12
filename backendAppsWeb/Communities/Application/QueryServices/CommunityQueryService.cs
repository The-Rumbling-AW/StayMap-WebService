

using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Communities.Domain.Model.Queries;
using backendAppsWeb.Communities.Domain.Repositories;
using backendAppsWeb.Communities.Domain.Services.Query;

namespace backendAppsWeb.Communities.Application.QueryServices;

public class CommunityQueryService(ICommunityRepository communityRepository) : ICommunityQueryService
{
    public async Task<Community?> Handle(GetCommunityById query)
    {
        return await communityRepository.FindDetailedByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Community>> Handle(GetCommunityByName query)
    {
        var communities = await communityRepository.GetCommunitiesByNameAsync(query.Name);
        return communities;
    }

    // Validación opcional: ¿ya existe comunidad con mismo nombre?
    public async Task<bool> ExistsCommunityWithSameName(string name)
    {
        var communities = await communityRepository.GetCommunitiesByNameAsync(name);
        return communities.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    public async Task<IEnumerable<Community>> Handle(GetAllCommunitiesQuery query)
    {
        return await communityRepository.ListAsync();
    }
    
}
