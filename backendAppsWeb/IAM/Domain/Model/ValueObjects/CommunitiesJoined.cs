namespace backendAppsWeb.IAM.Domain.Model.ValueObjects;

public record class CommunitiesJoined
{
    public List<int> CommunityIds { get; private set; }

    public CommunitiesJoined(List<int> communityIds)
    {
        CommunityIds = communityIds ?? new List<int>();
    }
}