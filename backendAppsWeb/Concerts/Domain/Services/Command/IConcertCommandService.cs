using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.Commands;

namespace backendAppsWeb.Concerts.Domain.Services.Command;

public interface IConcertCommandService
{
    Task<Concert?> Handle(CreateConcertCommand command);
    Task<bool> Handle(DeleteConcertCommand command);
    
    Task<bool> Handle(AttendConcertCommand command);
    
    Task<bool> ToggleAttendance(int concertId, int userId);

    Task<bool> CancelAttendance(int concertId, int userId);
    
    Task<bool> Handle(UpdateConcertStatusCommand command);

    
}