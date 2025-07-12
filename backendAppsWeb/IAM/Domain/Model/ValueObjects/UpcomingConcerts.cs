namespace backendAppsWeb.IAM.Domain.Model.ValueObjects;

public record class UpcomingConcerts
{
    public List<int> ConcertIds { get; private set; }

    public UpcomingConcerts(List<int> concertIds)
    {
        ConcertIds = concertIds ?? new List<int>();
    }
}