using backendAppsWeb.IAM.Domain.Model.Commands;
using backendAppsWeb.Posts.Domain.Model.Commands;
using backendAppsWeb.Posts.Domain.Model.Aggregates;

namespace backendAppsWeb.Posts.Domain.Services.Command;

public interface IPostCommandService
{
    Task<Post?> Handle(CreatePostCommand command);
    Task<bool> Handle(DeletePostCommand command);
    
    
}