namespace backendAppsWeb.Posts.Interfaces.Resources;

public class PostResource
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime PostedAt { get; set; }

    public string? ImageUrl { get; set; }  
    public int UserId { get; set; } 
    
}