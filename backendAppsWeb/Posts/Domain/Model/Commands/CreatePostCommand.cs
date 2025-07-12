namespace backendAppsWeb.Posts.Domain.Model.Commands;

public record CreatePostCommand(

    int CommunityId,
    string Content,
    int UserId,
    string? ImageUrl
    );