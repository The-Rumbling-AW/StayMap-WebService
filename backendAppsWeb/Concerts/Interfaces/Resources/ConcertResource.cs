using backendAppsWeb.Concerts.Domain.Model.ValueObjects;

namespace backendAppsWeb.Concerts.Interfaces.Resources;

public record ConcertResource

( 
    int Id,
    string Name ,
    string Genre,
    DateTime Date ,
    string Description,
    string Image,
    VenueResource Venue,
    string Status, 
    List<int> Attendees,
    string Platform 
    
    
);
