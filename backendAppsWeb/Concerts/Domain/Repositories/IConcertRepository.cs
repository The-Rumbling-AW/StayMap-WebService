using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;
using backendAppsWeb.Shared.Domain.Repositories;

namespace backendAppsWeb.Concerts.Domain.Repositories;

public interface IConcertRepository : IBaseRepository<Concert>
{
    Task<IEnumerable<Concert>> GetConcertsByDateAsync(DateTime date);
    Task<IEnumerable<Concert>> GetConcertsByNameAsync(string name);

    
    Task<Concert?> FindDetailedByIdAsync(int id);
    
    Task<IEnumerable<Concert>> ListAsync();
    
    Task<IEnumerable<Concert>> ListWithDetailsAsync();
    
    Task<IEnumerable<Concert>> FindByPlatformAsync(TicketPlatformType platform);

    
}