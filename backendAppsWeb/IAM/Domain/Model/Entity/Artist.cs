using backendAppsWeb.IAM.Domain.Model.ValueObjects;


namespace backendAppsWeb.IAM.Domain.Model.Entity;

public class Artist
{
    public bool HasCreatedConcerts { get; private set; } = true; // Campo obligatorio para EF Core

    public CreatedConcerts CreatedConcerts { get; private set; }
    
    public Artist(CreatedConcerts createdConcerts)
    {
        CreatedConcerts = createdConcerts ?? new CreatedConcerts(new List<int>());
    }

    private Artist() : this(new CreatedConcerts(new List<int>())) { }
    
}