namespace backendAppsWeb.Concerts.Interfaces.Resources;

public record CreateConcertResource
(
    string Name,
    string Genre,
    DateTime Date,
    string Description,
    string Image,
    string NameVenue,
    string Address,
    double Latitude,
    double Longitude,
    int Capacity,
    string Platform
   
);