using backendAppsWeb.Concerts.Interfaces.Resources;

namespace backendAppsWeb.Concerts.Interfaces.Transform;

public class VenueResourceFromEntityAssembler
{
    public static VenueResource toResourceFromEntity(VenueResource entity)
    {
        return new VenueResource(
            entity.Id,
            entity.NameVenue,
            entity.Address,
            entity.Location,
            entity.Capacity
        );
    }
}