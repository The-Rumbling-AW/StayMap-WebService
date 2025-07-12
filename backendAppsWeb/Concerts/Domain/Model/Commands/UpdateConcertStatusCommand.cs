using backendAppsWeb.Concerts.Domain.Model.ValueObjects;

namespace backendAppsWeb.Concerts.Domain.Model.Commands;

public record UpdateConcertStatusCommand(int ConcertId, Status NewStatus);