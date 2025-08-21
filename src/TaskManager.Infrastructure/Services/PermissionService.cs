using TaskManager.Application.Contracts;

namespace TaskManager.Infrastructure.Services;

public class PermissionService : IPermissionService
{
    public Task<bool> CanAccessProjectAsync(int userId, int projectId)
    {
        return Task.FromResult(true);
    }

    public Task<bool> CanEditTaskAsync(int userId, int taskId)
    {
        return Task.FromResult(true);
    }

    public Task<bool> IsProjectOwnerAsync(int userId, int projectId)
    {
        return Task.FromResult(true);
    }
}
