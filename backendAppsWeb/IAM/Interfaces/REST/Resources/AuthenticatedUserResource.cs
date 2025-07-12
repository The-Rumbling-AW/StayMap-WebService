using System.Text.Json.Serialization;
using backendAppsWeb.IAM.Domain.Model.ValueObjects;

namespace backendAppsWeb.IAM.Interfaces.REST.Resources;

public record AuthenticatedUserResource(
    int Id,
    string Username,
    string Name,
    string Email,
    string ProfileImage,
    ProfileType Type,
    List<int> PostsDone,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    List<int>? CreatedConcerts,
    string Token
);