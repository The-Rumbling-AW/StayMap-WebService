using backendAppsWeb.Communities.Interfaces.Resources;
using backendAppsWeb.Posts.Interfaces.Resources;

public record CommunityResource
(
    int Id,
    string Name,
 //   string MemberQuantity,
    string Image,
    string Description,
 //  List<string> Tags,
    List<int> Members,
  List<PostResource> Posts
    
);