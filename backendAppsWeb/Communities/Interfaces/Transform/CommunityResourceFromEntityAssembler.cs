using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Communities.Interfaces.Resources;
using backendAppsWeb.Posts.Interfaces.Resources;

namespace backendAppsWeb.Communities.Interfaces.Transform;

public static class CommunityResourceFromEntityAssembler
{
    public static CommunityResource toResourceFromEntity(Community community)
    {
        var postResources = community.Posts
            .Select(comment => new PostResource
            {
                Id = comment.Id,
                Content = comment.Content,
                PostedAt = comment.PostedAt,
                ImageUrl = comment.ImageUrl
            }).ToList();

        return new CommunityResource(
            community.Id,
            community.Name,
          //  community.MemberQuantity,
            community.Image,
            community.Description,
       //     community.Tags.Value,
       community.UserLinks?.Select(link => link.UserId).ToList() ?? new List<int>(),

            postResources
        );
    }
}