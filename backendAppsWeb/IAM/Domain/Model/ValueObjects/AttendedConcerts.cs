namespace backendAppsWeb.IAM.Domain.Model.ValueObjects;

public record class  AttendedConcerts
{
    public List<int> ConcertIds { get; private set; }

    public AttendedConcerts(List<int> concertIds)
    {
        ConcertIds = concertIds ?? new List<int>();
    }
}