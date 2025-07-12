using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.IAM.Domain.Model.Aggregate;

namespace backendAppsWeb.Posts.Domain.Model.Aggregates;

public partial class Post
{
    public int Id { get; set; }
    
    //public int UserId { get; private set; }
    
    public Community Community { get; private set; }
    public int CommunityId { get; private set; }
    
    
    
     public User User { get; private set; }  // opcional, solo si quieres la relación completa
    public int UserId { get; private set; } // ✅ NUEVO
   

    
    public string Content { get; set; }
    
    public DateTime PostedAt { get; set; }
    
    public string? ImageUrl { get; set; }
    
    public Post() { }
    
    //public Comment(int userId, int communityId, string content, string? imageUrl = null)
    public Post(Community community, int userId, string content, string? imageUrl = null)
    {
        Community = community ?? throw new ArgumentNullException(nameof(community));
        CommunityId = community.Id;

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("El contenido del comentario no puede estar vacío.", nameof(content));

        UserId = userId;
        Content = content;
        ImageUrl = imageUrl;
        PostedAt = DateTime.UtcNow;
    }
    
}