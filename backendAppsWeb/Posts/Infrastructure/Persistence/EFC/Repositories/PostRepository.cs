using backendAppsWeb.Posts.Domain.Repositories;
using backendAppsWeb.Posts.Domain.Model.Aggregates;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backendAppsWeb.Posts.Infrastructure.Persistence.EFC.Repositories;

public class PostRepository(AppDbContext dbContext): BaseRepository<Post>(dbContext), IPostsRepository
{
    
    
    
    public async Task<IEnumerable<Post>> FindByCommunityIdAsync(int communityId)
    {
        return await dbContext.Set<Post>()  // 👈 usa Set<T>() en lugar de dbContext.Posts
            .Where(p => p.CommunityId == communityId)
            .ToListAsync();
    }

    
    
}