namespace backendAppsWeb.IAM.Domain.Model.ValueObjects;

public record CreatedConcerts
{
    public List<int> ConcertIds { get; private set; }

    // Constructor principal
    public CreatedConcerts(List<int> concertIds)
    {
        ConcertIds = concertIds ?? new List<int>();
    }

    // Constructor sin parámetros (necesario para EF Core)
    public CreatedConcerts() : this(new List<int>()) { }

    // Método para agregar un concierto
    public void Add(int concertId)
    {
        if (!ConcertIds.Contains(concertId))
        {
            ConcertIds.Add(concertId);
        }
    }
}