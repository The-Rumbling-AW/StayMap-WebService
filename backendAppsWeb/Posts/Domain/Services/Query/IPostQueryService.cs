using backendAppsWeb.Posts.Domain.Model.Aggregates;
using backendAppsWeb.Posts.Domain.Model.Queries;

namespace backendAppsWeb.Posts.Domain.Services.Query;

public interface IPostQueryService
{
    Task<Post?> Handle(GetPostById query);
    Task<IEnumerable<Post>> Handle(GetPostsByCommunityId query); // 👈 importante que sea ese nombre
}