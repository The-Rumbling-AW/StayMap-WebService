using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.IAM.Domain.Model.Aggregate;

namespace backendAppsWeb.Concerts.Domain.Model.Entity;


public class ConcertAttendee
{
    public int ConcertId { get; set; }
    public Concert Concert { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
}