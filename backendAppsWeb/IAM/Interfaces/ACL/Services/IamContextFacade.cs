using backendAppsWeb.IAM.Domain.Model.Commands;
using backendAppsWeb.IAM.Domain.Model.Queries;
using backendAppsWeb.IAM.Domain.Model.ValueObjects;
using backendAppsWeb.IAM.Domain.Services;


namespace backendAppsWeb.IAM.Interfaces.ACL.Services;

public class IamContextFacade(IUserCommandService userCommandService, IUserQueryService userQueryService) : IIamContextFacade
{
    public async Task<int> CreateUser(string username, string password, string name, string email, string profileImage, ProfileType type)
    {
        var signUpCommand = new SignUpCommand(username, password, name, email, profileImage, type);
        await userCommandService.Handle(signUpCommand);
    
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<int> FetchUserIdByUsername(string username)
    {
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserId(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Username ?? string.Empty;
    }
}