using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.IAM.Domain.Model.ValueObjects;
using backendAppsWeb.IAM.Interfaces.REST.Resources;


namespace backendAppsWeb.IAM.Interfaces.REST.Transform;

public static class UserResourceFromEntityAssembler
{
    public static object ToResourceFromEntity(User entity)
    {
        var communitiesJoined = entity.CommunityLinks?.Select(cl => cl.CommunityId).ToList() ?? new List<int>();
        var postsDone = entity.PostsDone?.PostIds ?? new List<int>();
        var attendedConcerts = entity.ConcertLinks?.Select(ca => ca.ConcertId).ToList() ?? new List<int>();
        var likedPosts = entity.LikedPosts?.PostIds ?? new(); // ✅ Extraer los likes

        if (entity.Type == ProfileType.Artist)
        {
            var createdConcerts = entity.Artist?.CreatedConcerts?.ConcertIds ?? new List<int>();

            return new ArtistProfileResource(
                entity.Id,
                entity.Name,
                entity.Email.Address,
                entity.ProfileImage,
                entity.Type,
                communitiesJoined,
                postsDone,
                createdConcerts,
                attendedConcerts,
                entity.Username,
                likedPosts
                
            );
        }

        return new FanProfileResource(
            entity.Id,
            entity.Name,
            entity.Email.Address,
            entity.ProfileImage,
            entity.Type,
            communitiesJoined,
            postsDone,
            attendedConcerts,
            entity.Username,
            likedPosts
        );
    }
}