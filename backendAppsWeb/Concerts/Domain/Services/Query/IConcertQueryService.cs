using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.Queries;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;

namespace backendAppsWeb.Concerts.Domain.Services.Query;

public interface IConcertQueryService
{
    Task<Concert?> Handle(GetConcertById query);
    Task<IEnumerable<Concert>> Handle(GetConcertByDate query);
    Task<IEnumerable<Concert>> Handle(GetConcertByName query);
    Task<IEnumerable<Concert>> Handle(GetAllConcerts query);
    
    Task<IEnumerable<Concert>> GetByPlatformAsync(TicketPlatformType platform);

}