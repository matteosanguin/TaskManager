namespace TaskManager.Application.Contracts;

public interface IPermissionService
{
    Task<bool> CanAccessProjectAsync(int userId, int projectId);
    Task<bool> CanEditTaskAsync(int userId, int taskId);
    Task<bool> IsProjectOwnerAsync(int userId, int projectId);
}
