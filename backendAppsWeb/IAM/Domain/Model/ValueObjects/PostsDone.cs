namespace backendAppsWeb.IAM.Domain.Model.ValueObjects;

public record class PostsDone
{
    public List<int> PostIds { get; private set; }

    public PostsDone(List<int> postIds)
    {
        PostIds = postIds ?? new List<int>();
    }
    
    
    public void Add(int postId)
    {
        if (!PostIds.Contains(postId))
            PostIds.Add(postId);
    }
}