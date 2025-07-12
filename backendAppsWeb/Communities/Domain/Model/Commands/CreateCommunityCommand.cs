namespace backendAppsWeb.Communities.Domain.Model.Commands;

public record CreateCommunityCommand(
    string Name,
//    string MemberQuantity,
    string Image,
    string Description,
 //   List<string> Tags,
    List<int> Members

);
