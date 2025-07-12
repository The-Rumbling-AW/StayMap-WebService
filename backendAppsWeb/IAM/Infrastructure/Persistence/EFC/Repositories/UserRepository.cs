using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.IAM.Domain.Repositories;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Configuration;
using backendAppsWeb.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace backendAppsWeb.IAM.Infrastructure.Persistence.EFC.Repositories;

public class UserRepository(AppDbContext context) 
    : BaseRepository<User>(context), IUserRepository
{
    public async Task<User?> FindByUsernameAsync(string username)
    {
        return await Context.Set<User>()
            .Include(u => u.CommunityLinks)
            .Include(u => u.ConcertLinks)
            .Include(u => u.PostsDone)
            .Include(u => u.Artist!.CreatedConcerts)
            .FirstOrDefaultAsync(user => user.Username.Equals(username));
    }


    public bool ExistsByUsername(string username)
    {
        return Context.Set<User>()
            .Any(user => user.Username.Equals(username));
    }

    // ✅ Método que reemplaza GetExistingProfileIdsAsync pero para usuarios
    public async Task<List<int>> GetExistingUserIdsAsync(IEnumerable<int> ids)
    {
        return await Context.Set<User>()
            .Where(u => ids.Contains(u.Id))
            .Select(u => u.Id)
            .ToListAsync();
    }
    
    public async Task<User?> FindByIdAsync(int id)
    {
        return await Context.Set<User>()
            .Include(u => u.CommunityLinks)
            .Include(u => u.ConcertLinks)
            .Include(u => u.PostsDone)
            .Include(u => u.Artist!.CreatedConcerts) // Solo si Artist no es null
            .FirstOrDefaultAsync(u => u.Id == id);
    }
    
    
    public async Task<IEnumerable<User>> ListAsync()
    {
        return await Context.Set<User>()
            .Include(u => u.CommunityLinks)
            .Include(u => u.ConcertLinks)
            .Include(u => u.PostsDone)
            .Include(u => u.Artist!.CreatedConcerts)
            .ToListAsync();
    }


    
}