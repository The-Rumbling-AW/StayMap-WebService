using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Posts.Domain.Model.Aggregates;
using backendAppsWeb.Shared.Domain.Repositories;

namespace backendAppsWeb.Posts.Domain.Repositories;

public interface IPostsRepository: IBaseRepository<Post>
{
    
    Task<IEnumerable<Post>> FindByCommunityIdAsync(int communityId);

    
}