using Microsoft.EntityFrameworkCore;
using backendAppsWeb.Concerts.Domain.Model.Aggregates;
using backendAppsWeb.Concerts.Domain.Model.ValueObjects;
using backendAppsWeb.Concerts.Domain.Repositories;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Repositories;

namespace backendAppsWeb.Concerts.Infrastructure.Persistence.EFC.Repositories;

public class ConcertRepository(AppDbContext context)
    : BaseRepository<Concert>(context), IConcertRepository
{
    public async Task<IEnumerable<Concert>> GetConcertsByDateAsync(DateTime date)
    {
        return await Context.Set<Concert>()
            .Include(c => c.Venue)
            .Where(c => c.Date.Date == date.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Concert>> GetConcertsByNameAsync(string name)
    {
        return await Context.Set<Concert>()
            .Include(c => c.Venue)
            .Where(c => c.Name.ToLower() == name.ToLower())
            .ToListAsync();
    }

    // NUEVO MÉTODO para reemplazar el FindByIdAsync del base cuando necesites Includes
    public async Task<Concert?> FindDetailedByIdAsync(int id)
    {
        return await Context.Set<Concert>()
            .Include(c => c.Venue)
            .Include(c => c.AttendeeLinks) // <--- Asegúrate de incluir esto
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    // OVERRIDE PARA INCLUIR VENUE EN GETALL
    public async Task<IEnumerable<Concert>> ListWithDetailsAsync()
    {
        return await Context.Set<Concert>()
            .Include(c => c.Venue)
            .Include(c => c.AttendeeLinks)
            .ToListAsync();
    }

    
   
    
    public async Task<IEnumerable<Concert>> FindByPlatformAsync(TicketPlatformType platform)
    {
        return await Context.Set<Concert>()
            .Where(c => c.Platform == platform)
            .Include(c => c.Venue)
            .Include(c => c.AttendeeLinks)
            .ToListAsync();
    }

    


}