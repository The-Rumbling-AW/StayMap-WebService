using backendAppsWeb.IAM.Domain.Model.Commands;
using backendAppsWeb.IAM.Interfaces.REST.Resources;

namespace backendAppsWeb.IAM.Interfaces.REST.Transform;

public class SignInCommandFromResourceAssembler
{
    public static SignInCommand ToCommandFromResource(SignInResource resource)
    {
        return new SignInCommand(resource.Username, resource.Password);
    }
}