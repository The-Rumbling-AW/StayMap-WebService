using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.Queries;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;
using backendAppsWeb.Concerts.Domain.Repositories;
using backendAppsWeb.Concerts.Domain.Services.Query;

namespace backendAppsWeb.Concerts.Application.QueryService;

public class ConcertQueryService(IConcertRepository concertRepository) : IConcertQueryService
{
    public async Task<Concert?> Handle(GetConcertById query)
    {
        return await concertRepository.FindDetailedByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Concert>> Handle(GetConcertByDate query)
    {
        var concerts = await concertRepository.GetConcertsByDateAsync(query.Date);
        return concerts;
    }

    public async Task<IEnumerable<Concert>> Handle(GetConcertByName query)
    {
        var concerts = await concertRepository.GetConcertsByNameAsync(query.Name);
        return concerts;
    }

    public async Task<IEnumerable<Concert>> Handle(GetAllConcerts query)
    {
        return await concertRepository.ListWithDetailsAsync(); // ✅ nuevo
    }


    public async Task<bool> ExistsConcertInSameVenueOnSameDate(string venueName, DateTime date)
    {
        var concerts = await concertRepository.GetConcertsByDateAsync(date);
        return concerts.Any(c => c.Venue.Namevenue == venueName);
    }

    public async Task<bool> ExistsConcertWithSameNameOnSameDate(string name, DateTime date)
    {
        var concerts = await concertRepository.GetConcertsByDateAsync(date);
        return concerts.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
    
    public async Task<IEnumerable<Concert>> GetByPlatformAsync(TicketPlatformType platform)
    {
        return await concertRepository.FindByPlatformAsync(platform);
    }

    
}