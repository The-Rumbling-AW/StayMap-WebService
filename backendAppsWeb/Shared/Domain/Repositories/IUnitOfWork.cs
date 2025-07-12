namespace backendAppsWeb.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}