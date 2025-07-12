using backendAppsWeb.Posts.Domain.Model.Aggregates;
using backendAppsWeb.Posts.Interfaces.Resources;

namespace backendAppsWeb.Posts.Interfaces.Transform;

public static class PostResourceFromEntityAssembler
{
    public static PostResource ToResourceFromEntity(Post post)
    {
        return new PostResource
        {
            Id = post.Id,
            Content = post.Content,
            PostedAt = post.PostedAt,
            ImageUrl = post.ImageUrl,
            UserId = post.UserId 
        };
    }
}