using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.IAM.Domain.Model.ValueObjects;
using backendAppsWeb.IAM.Interfaces.REST.Resources;

namespace backendAppsWeb.IAM.Interfaces.REST.Transform;

public static class AuthenticatedUserResourceFromEntityAssembler
{
    public static AuthenticatedUserResource ToResourceFromEntity(User user, string token)
    {
        return new AuthenticatedUserResource(
            user.Id,
            user.Username,
            user.Name,
            user.Email.Address,
            user.ProfileImage,
            user.Type,
            user.PostsDone.PostIds,
            user.Type == ProfileType.Artist ? user.Artist?.CreatedConcerts.ConcertIds : null,
            token
        );
    }
}