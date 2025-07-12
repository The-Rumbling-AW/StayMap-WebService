using backendAppsWeb.Concerts.Domain.Model.ValueObjects;

namespace backendAppsWeb.Concerts.Interfaces.Resources;

public record VenueResource(
    int Id,
    string NameVenue,
    string Address,
    Location Location,
    int Capacity
    );