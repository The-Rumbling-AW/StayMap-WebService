using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Interfaces.Resources;

namespace backendAppsWeb.Concerts.Interfaces.Transform;

public static class ConcertResourceFromEntityAssembler
{
    public static ConcertResource toResourceFromEntity(Concert concert)
    {
        return new ConcertResource(
            concert.Id,
            concert.Name,
            concert.Genre.ToString(),
            concert.Date,
            concert.Description,
            concert.Image,
            new VenueResource(
                concert.Venue.Id,
                concert.Venue.Namevenue,
                concert.Venue.Address,
                concert.Venue.Location,
                concert.Venue.Capacity
            ),
            concert.Status.ToString(), // ✅ aquí agregas el status como string
            concert.AttendeeLinks?.Select(a => a.UserId).ToList() ?? new List<int>(),
            concert.Platform.ToString() // 👈 aquí convertimos el enum a texto
        );


    }
}