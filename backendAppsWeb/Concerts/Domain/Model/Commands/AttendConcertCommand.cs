namespace backendAppsWeb.Concerts.Domain.Model.Commands;

public record AttendConcertCommand(int ConcertId, int UserId);