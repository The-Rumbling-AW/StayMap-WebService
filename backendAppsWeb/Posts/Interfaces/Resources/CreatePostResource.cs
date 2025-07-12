namespace backendAppsWeb.Posts.Interfaces.Resources;

public record CreatePostResource(
  
    string Content,
    string? ImageUrl
);