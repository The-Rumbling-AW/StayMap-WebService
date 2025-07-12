using backendAppsWeb.IAM.Domain.Model.Aggregate;
using backendAppsWeb.IAM.Domain.Model.Queries;

namespace backendAppsWeb.IAM.Domain.Services;

public interface IUserQueryService
{
    Task<User?> Handle(GetUserByUsernameQuery query);
    
    Task<User?> Handle(GetUserByIdQuery query);
    
    Task<IEnumerable<User>> Handle(GetAllUsersQuery query);
}