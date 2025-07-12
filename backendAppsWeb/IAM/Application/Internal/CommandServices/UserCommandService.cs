using backendAppsWeb.IAM.Application.Internal.OutboundServices;
using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.IAM.Domain.Model.Commands;
using backendAppsWeb.IAM.Domain.Model.ValueObjects;
using backendAppsWeb.IAM.Domain.Repositories;
using backendAppsWeb.IAM.Domain.Services;

using backendAppsWeb.Shared.Domain.Repositories;

namespace backendAppsWeb.IAM.Application.Internal.CommandServices;

public class UserCommandService(
    IUserRepository userRepository,
    ITokenService tokenService,
    IHashingService hashingService,
    IUnitOfWork unitOfWork)
    : IUserCommandService
{
    /**
     * <summary>
     *     Handle sign in command
     * </summary>
     * <param name="command">The sign in command</param>
     * <returns>The authenticated user and the JWT token</returns>
     */
    public async Task<(User user, string token)> Handle(SignInCommand command)
    {
        var user = await userRepository.FindByUsernameAsync(command.Username);

        if (user == null || !hashingService.VerifyPassword(command.Password, user.PasswordHash))
            throw new Exception("Invalid username or password");

        var token = tokenService.GenerateToken(user);

        return (user, token);
    }

    /**
     * <summary>
     *     Handle sign up command
     * </summary>
     * <param name="command">The sign up command</param>
     * <returns>A confirmation message on successful creation.</returns>
     */
    public async Task Handle(SignUpCommand command)
    {
        if (userRepository.ExistsByUsername(command.Username))
            throw new Exception($"Username {command.Username} is already taken");

        var hashedPassword = hashingService.HashPassword(command.Password);
        var email = new EmailAddress(command.Email);

        var user = new User(
            command.Username,
            hashedPassword,
            command.Name,
            email,
            command.ProfileImage,
            command.Type
        );

        try
        {
            await userRepository.AddAsync(user);
            await unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"An error occurred while creating user: {e.Message}");
        }
    }
    
    public async Task<bool> Handle(LikePostCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null) return false;

        user.LikedPosts.Add(command.PostId);
        await unitOfWork.CompleteAsync();
        return true;
    }

    
    public async Task<bool> Handle(UpdateUserCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.Id);
        if (user == null) return false;

        user.UpdateProfile(command.Name, command.Email, command.ProfileImage);

        await unitOfWork.CompleteAsync();
        return true;
    }
    
    
    public async Task<bool> Handle(DeleteLikePostCommand command)
    {
        var user = await userRepository.FindByIdAsync(command.UserId);
        if (user == null || user.LikedPosts == null) return false;

        if (user.LikedPosts.Contains(command.PostId))
        {
            user.LikedPosts.Remove(command.PostId);
            await unitOfWork.CompleteAsync();
            return true;
        }

        return false;
    }

}
