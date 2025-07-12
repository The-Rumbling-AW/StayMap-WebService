
using backendAppsWeb.IAM.Domain.Model.ValueObjects;

namespace backendAppsWeb.IAM.Interfaces.REST.Resources;

public record UserResource(
    int Id,
    string Username,
    string Name,
    string Email,
    string ProfileImage,
    ProfileType Type,
    List<int> CommunitiesJoined, // ahora sí, desde tabla intermedia
    List<int> PostsDone,
    
    List<int>? CreatedConcerts, // solo si es artista
    List<int> LikedPosts // 👈 AÑADIDO
);