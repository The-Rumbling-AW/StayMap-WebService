namespace backendAppsWeb.Communities.Domain.Model.Commands;

public record LeaveCommunityCommand(int CommunityId, int UserId);

