namespace backendAppsWeb.Communities.Domain.Model.Commands;

public record JoinCommunityCommand(int CommunityId, int UserId);