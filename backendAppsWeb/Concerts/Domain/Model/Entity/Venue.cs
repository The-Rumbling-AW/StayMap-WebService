using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;

namespace backendAppsWeb.Concerts.Domain.Model.Entity;

public class Venue
{
    public int Id { get; }
    public string? Namevenue { get; set; }
    public string? Address { get; set; }
    public Location Location { get; set; }  
    public int Capacity { get; set; }

    public Venue(string namevenue, string address, double latitude, double longitude, int capacity)
    {
        if (string.IsNullOrWhiteSpace(namevenue))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(namevenue));
        
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be null or whitespace.", nameof(address));

        if (capacity <= 0)
            throw new ArgumentException("Capacity must be greater than zero.", nameof(capacity));

        Namevenue = namevenue;
        Address = address;
        Capacity = capacity;
        Location = new Location(latitude, longitude);  
    }

   
    private Venue() { }

    //One to Many
    public ICollection<Concert> Concerts { get; set; } = new List<Concert>();
  
}