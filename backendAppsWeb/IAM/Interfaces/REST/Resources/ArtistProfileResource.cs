

using backendAppsWeb.IAM.Domain.Model.ValueObjects;

namespace backendAppsWeb.IAM.Interfaces.REST.Resources;

public record ArtistProfileResource(
    int Id,
    string Name,
    string Email,
    string ProfileImage,
    ProfileType Type,
    List<int> CommunitiesJoined,
    List<int> PostsDone,
    List<int> CreatedConcerts,
    List<int> AttendedConcerts,
  
    string Username,
    List<int> LikedPosts// 👈 AÑADIDO
);
