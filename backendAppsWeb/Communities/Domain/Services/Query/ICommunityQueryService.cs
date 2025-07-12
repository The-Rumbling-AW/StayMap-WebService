using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Communities.Domain.Model.Queries;

namespace backendAppsWeb.Communities.Domain.Services.Query;

public interface ICommunityQueryService
{
    Task<Community?> Handle(GetCommunityById query);
    Task<IEnumerable<Community>> Handle(GetCommunityByName query);
    
    Task<IEnumerable<Community>> Handle(GetAllCommunitiesQuery query);
}