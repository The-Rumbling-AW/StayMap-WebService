using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.IAM.Domain.Model.Aggregate;

namespace backendAppsWeb.Communities.Domain.Model.Entity;

public class CommunityMember
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int CommunityId { get; set; }
    public Community Community { get; set; }
}