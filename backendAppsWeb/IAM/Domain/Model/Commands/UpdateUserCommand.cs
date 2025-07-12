namespace backendAppsWeb.IAM.Domain.Model.Commands;

public record UpdateUserCommand(int Id, string Name, string Email, string ProfileImage);