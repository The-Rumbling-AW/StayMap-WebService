using System.Text.Json.Serialization;
using backendAppsWeb.Communities.Domain.Model.Entity;
using backendAppsWeb.Concerts.Domain.Model.Entity;
using backendAppsWeb.IAM.Domain.Model.Entity;
using backendAppsWeb.IAM.Domain.Model.ValueObjects;



namespace backendAppsWeb.IAM.Domain.Model.Aggregate;


public class User
{
    public int Id { get; private set; }

    // Credenciales
    public string Username { get; private set; }
    
    [JsonIgnore]
    public string PasswordHash { get; private set; }

    // Información de perfil
    public string Name { get; private set; }
    public EmailAddress Email { get; private set; }
    public string ProfileImage { get; private set; }
    public ProfileType Type { get; private set; }
    
    public LikedPosts LikedPosts { get; private set; }


    // Value Objects del dominio

    
    public ICollection<ConcertAttendee> ConcertLinks { get; set; } = new List<ConcertAttendee>();
    
    public ICollection<CommunityMember> CommunityLinks { get; set; } = new List<CommunityMember>();

    
    public PostsDone PostsDone { get; private set; }

    // Fan o Artist (según tipo)
    public Fan? Fan { get; private set; }
    public Artist? Artist { get; private set; }

    // Constructor sin parámetros (EF Core)
    private User() {}

    public User(string username, string passwordHash, string name, EmailAddress email, string profileImage, ProfileType type)
    {
        if (string.IsNullOrWhiteSpace(username)) throw new ArgumentException("Username is required");
        if (string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password hash is required");

        Username = username;
        PasswordHash = passwordHash;
        Name = name;
        Email = email;
        ProfileImage = profileImage;
        Type = type;


        PostsDone = new PostsDone(new());
        LikedPosts = new LikedPosts(new()); // ← ✅ FALTA ESTO

        if (type == ProfileType.Fan)
            Fan = new Fan();
        else if (type == ProfileType.Artist)
            Artist = new Artist(new CreatedConcerts(new()));
    }
    
    public void UpdateProfile(string name, string email, string profileImage)
    {
        Name = name;
        Email = new EmailAddress(email);
        ProfileImage = profileImage;
    }


 
}