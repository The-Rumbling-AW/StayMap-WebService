namespace backendAppsWeb.Communities.Domain.Model.ValueObjects;

public record Members
{
    public List<int> UserIds { get; private set; }

    public Members(List<int> userIds)
    {
        UserIds = userIds ?? new List<int>();
    }

}
