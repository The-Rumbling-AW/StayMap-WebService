using backendAppsWeb.Posts.Domain.Model.Aggregates;
using backendAppsWeb.Posts.Domain.Model.Queries;
using backendAppsWeb.Posts.Domain.Repositories;
using backendAppsWeb.Posts.Domain.Services.Query;

namespace backendAppsWeb.Posts.Application.QueryService;

public class PostQueryService(IPostsRepository postRepository) : IPostQueryService
{
    public async Task<Post?> Handle(GetPostById query)
    {
        return await postRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<Post>> Handle(GetPostsByCommunityId query)
    {
        return await postRepository.FindByCommunityIdAsync(query.CommunityId);
    }
}