using backendAppsWeb.Communities.Domain.Model.Aggregates;
using backendAppsWeb.Communities.Domain.Model.Commands;

namespace backendAppsWeb.Communities.Domain.Services.Command;

public interface ICommunityCommandService
{
    Task<Community?> Handle(CreateCommunityCommand command);
    Task<bool> Handle(DeleteCommunityCommand command); 
    
    Task<bool> Handle(JoinCommunityCommand command);
    
    Task<bool> Handle(LeaveCommunityCommand command);
}