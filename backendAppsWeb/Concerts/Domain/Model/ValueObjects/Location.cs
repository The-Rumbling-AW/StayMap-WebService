namespace backendAppsWeb.Concerts.Domain.Model.ValueObjects;

public record Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    private Location() { } 
}