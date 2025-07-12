using backendAppsWeb.IAM.Domain.Model.ValueObjects;


namespace backendAppsWeb.IAM.Interfaces.ACL;

public interface IIamContextFacade
{
    Task<int> CreateUser(
        string username,
        string password,
        string name,
        string email,
        string profileImage,
        ProfileType type);
    Task<int> FetchUserIdByUsername(string username);
    Task<string> FetchUsernameByUserId(int userId);
}