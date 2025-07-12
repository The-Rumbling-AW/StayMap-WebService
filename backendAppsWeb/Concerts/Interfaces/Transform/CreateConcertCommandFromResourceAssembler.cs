using backendAppsWeb.Concerts.Domain.Model.Commands;
using backendAppsWeb.Concerts.Interfaces.Resources;

namespace backendAppsWeb.Concerts.Interfaces.Transform;

public static class CreateConcertCommandFromResourceAssembler
{
    public static CreateConcertCommand toCommandFromResource(CreateConcertResource resource)
    {
        return new CreateConcertCommand(
            resource.Name,
            resource.Genre,
            resource.Date,
            resource.Description,
            resource.Image,
            resource.NameVenue,
            resource.Address,
            resource.Latitude,
            resource.Longitude,
            resource.Capacity,
        new List<int>(),
            resource.Platform// ✅ ya no usamos resource.Attendees
            );
    }
}

