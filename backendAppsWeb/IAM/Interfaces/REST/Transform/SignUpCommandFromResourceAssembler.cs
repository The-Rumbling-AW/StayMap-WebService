using backendAppsWeb.IAM.Domain.Model.Commands;
using backendAppsWeb.IAM.Interfaces.REST.Resources;

namespace backendAppsWeb.IAM.Interfaces.REST.Transform;

public static class SignUpCommandFromResourceAssembler
{
    public static SignUpCommand ToCommandFromResource(SignUpResource resource)
    {
        return new SignUpCommand(
            resource.Username,
            resource.Password,
            resource.Name,
            resource.Email,
            resource.ProfileImage,
            resource.Type
        );
    }
}