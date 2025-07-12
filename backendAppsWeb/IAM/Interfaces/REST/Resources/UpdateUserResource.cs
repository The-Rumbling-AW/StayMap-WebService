namespace backendAppsWeb.IAM.Interfaces.REST.Resources;

public record UpdateUserResource(
    string Name,
    string Email,
    string ProfileImage
);