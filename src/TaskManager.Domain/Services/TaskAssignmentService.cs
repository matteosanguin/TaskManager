using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Services;

/// <summary>
/// Implementazione del servizio di assegnazione task
/// </summary>
public class TaskAssignmentService : ITaskAssignmentService
{
    private readonly IProjectPermissionService _projectPermissionService;

    public TaskAssignmentService(IProjectPermissionService projectPermissionService)
    {
        _projectPermissionService = projectPermissionService;
    }

    /// <summary>
    /// Assegna un task a un utente
    /// </summary>
    /// <param name="task">Il task da assegnare</param>
    /// <param name="user">L'utente a cui assegnare il task</param>
    /// <returns>True se l'assegnazione è avvenuta con successo, false altrimenti</returns>
    public bool AssignTask(DomainTask task, User user)
    {
        // Verifica se l'utente può essere assegnato al task
        if (!CanAssignUserToTask(task, user))
        {
            return false;
        }

        try
        {
            // Assegna l'utente al task
            task.AssigneeId = user.Id;
            task.Assignee = user;

            // Aggiorna la data di modifica
            task.ModifiedAt = DateTime.UtcNow;

            return true;
        }
        catch
        {
            // In caso di errore, ripristina lo stato originale
            task.AssigneeId = null;
            task.Assignee = null;
            return false;
        }
    }

    /// <summary>
    /// Rimuove l'assegnazione di un task
    /// </summary>
    /// <param name="task">Il task di cui rimuovere l'assegnazione</param>
    /// <returns>True se la rimozione è avvenuta con successo, false altrimenti</returns>
    public bool UnassignTask(DomainTask task)
    {
        try
        {
            // Rimuovi l'assegnazione
            task.AssigneeId = null;
            task.Assignee = null;

            // Aggiorna la data di modifica
            task.ModifiedAt = DateTime.UtcNow;

            return true;
        }
        catch
        {
            // In caso di errore, non è possibile ripristinare lo stato originale
            // perché non abbiamo salvato il valore precedente
            return false;
        }
    }

    /// <summary>
    /// Verifica se un utente può essere assegnato a un task
    /// </summary>
    /// <param name="task">Il task</param>
    /// <param name="user">L'utente</param>
    /// <returns>True se l'utente può essere assegnato al task, false altrimenti</returns>
    public bool CanAssignUserToTask(DomainTask task, User user)
    {
        // Verifica che il task e l'utente non siano null
        if (task == null || user == null)
        {
            return false;
        }

        // Verifica che l'utente abbia accesso al progetto del task
        if (!_projectPermissionService.CanReadProject(user, task.Project))
        {
            return false;
        }

        // Verifica che l'utente non sia già assegnato al task
        if (IsUserAssignedToTask(task, user))
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Verifica se un utente è assegnato a un task
    /// </summary>
    /// <param name="task">Il task</param>
    /// <param name="user">L'utente</param>
    /// <returns>True se l'utente è assegnato al task, false altrimenti</returns>
    public bool IsUserAssignedToTask(DomainTask task, User user)
    {
        // Verifica che il task e l'utente non siano null
        if (task == null || user == null)
        {
            return false;
        }

        // Verifica se l'ID dell'assegnatario corrisponde all'ID dell'utente
        return task.AssigneeId.HasValue && task.AssigneeId.Value == user.Id;
    }
}
