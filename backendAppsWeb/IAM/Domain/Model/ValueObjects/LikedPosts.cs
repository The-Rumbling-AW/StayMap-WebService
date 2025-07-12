namespace backendAppsWeb.IAM.Domain.Model.ValueObjects;

public class LikedPosts
{
    public List<int> PostIds { get; private set; }

    public LikedPosts(List<int> postIds)
    {
        PostIds = postIds ?? new List<int>();
    }

    public void Add(int postId)
    {
        if (!PostIds.Contains(postId))
            PostIds.Add(postId);
    }
    
    public void Remove(int postId)
    {
        PostIds.Remove(postId); 
    }

    public bool Contains(int postId)
    {
        return PostIds.Contains(postId);
    }
    
}