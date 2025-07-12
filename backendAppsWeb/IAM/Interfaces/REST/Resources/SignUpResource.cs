using backendAppsWeb.IAM.Domain.Model.ValueObjects;


namespace backendAppsWeb.IAM.Interfaces.REST.Resources;

public record SignUpResource(
    string Username,
    string Password,
    string Name,
    string Email,
    string ProfileImage,
    ProfileType Type
);