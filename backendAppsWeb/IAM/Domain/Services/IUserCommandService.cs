using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.IAM.Domain.Model.Commands;

namespace backendAppsWeb.IAM.Domain.Services;

public interface IUserCommandService
{
    Task<(User user, string token)> Handle(SignInCommand signInCommand);
    Task Handle(SignUpCommand signUpCommand);
    
    Task<bool> Handle(LikePostCommand command);

    Task<bool> Handle(UpdateUserCommand command);
    
    Task<bool> Handle(DeleteLikePostCommand command);

}