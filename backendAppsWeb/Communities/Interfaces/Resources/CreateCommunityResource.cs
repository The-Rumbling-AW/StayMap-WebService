
namespace backendAppsWeb.Communities.Interfaces.Resources;

public record CreateCommunityResource(
    string Name,
   // string MemberQuantity,
    string Image,
    //**
    string Description,
   //List<string> Tags,
   List<int> Members
   //List<CommentResource> Comments

    );