using backendAppsWeb.Posts.Domain.Model.Commands;
using backendAppsWeb.Posts.Interfaces.Resources;

namespace backendAppsWeb.Posts.Interfaces.Transform;

public static class CreatePostCommandFromResourceAssembler
{
    public static CreatePostCommand ToCommand(int communityId, int userId, CreatePostResource resource)
        => new(communityId, resource.Content, userId, resource.ImageUrl);
}
