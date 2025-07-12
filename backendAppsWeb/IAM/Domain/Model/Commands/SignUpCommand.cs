using backendAppsWeb.IAM.Domain.Model.ValueObjects;

namespace backendAppsWeb.IAM.Domain.Model.Commands;

public record SignUpCommand(
    string Username, string Password,
    string Name,
    string Email,
    string ProfileImage,
    ProfileType Type
    );