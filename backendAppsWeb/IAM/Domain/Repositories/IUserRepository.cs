using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.Shared.Domain.Repositories;

namespace backendAppsWeb.IAM.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByUsernameAsync(string username);          
    bool ExistsByUsername(string username);                    
    Task<List<int>> GetExistingUserIdsAsync(IEnumerable<int> ids); 
    
    Task<User?> FindByIdAsync(int id);

}
