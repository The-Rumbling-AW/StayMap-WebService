using backendAppsWeb.Concerts.Domain.Model.ValueObjects;

namespace backendAppsWeb.Concerts.Domain.Model.Commands;

public record CreateConcertCommand(
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
    List<int>? Attendees, 
    string Platform 
);
