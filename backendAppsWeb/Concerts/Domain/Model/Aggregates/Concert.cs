using backendAppsWeb.Concerts.Domain.Model.Entity;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;

namespace backendAppsWeb.Concerts.Domain.Model.Aggregates;


public partial class Concert
{
    public int Id { get; private set; }
    public string Name { get; set; }
    public Genre Genre { get; set; }
    public DateTime Date { get; set; }
    public Status Status { get; set; }
    public string Description { get; set; }
    public string Image { get; set; }
    public Venue Venue { get; set; }
    
    public ICollection<ConcertAttendee> AttendeeLinks { get; set; } = new List<ConcertAttendee>();

    public TicketPlatformType Platform { get; set; }

 

    
    //
    
    
    public Concert() { } 
    public Concert(string name, string genre, DateTime date, string description, string image, 
        string namevenue, string address, double latitude, double longitude, int capacity,  string platform)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio.", nameof(name));

        if (name.Length > 30)
            throw new ArgumentException("El nombre no puede exceder los 30 caracteres.", nameof(name));

        if (string.IsNullOrWhiteSpace(genre))
            throw new ArgumentException("El género es obligatorio.", nameof(genre));

        if (!Enum.TryParse<Genre>(genre, ignoreCase: true, out var parsedGenre))
            throw new ArgumentException("El género no es válido.", nameof(genre));

        if (date == default)
            throw new ArgumentException("La fecha del concierto es obligatoria.", nameof(date));

        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("La descripción es obligatoria.", nameof(description));

        if (description.Length > 500)
            throw new ArgumentException("La descripción no puede exceder los 500 caracteres.", nameof(description));

        if (string.IsNullOrWhiteSpace(image))
            throw new ArgumentException("La imagen es obligatoria.", nameof(image));
        
        if (string.IsNullOrWhiteSpace(platform))
            throw new ArgumentException("La plataforma es obligatoria.", nameof(platform));
        
        if (!Enum.TryParse<TicketPlatformType>(platform, ignoreCase: true, out var parsedPlatform))
            throw new ArgumentException("La plataforma no es válida. Usa: Teleticket, Ticketmaster o Joinnus.", nameof(platform));
        
        Name = name;
        Genre = parsedGenre;
        Date = date;
        Description = description;
        Image = image;
        Platform = parsedPlatform;
        Venue=new Venue(namevenue, address, latitude, longitude, capacity);
        
    }
}
