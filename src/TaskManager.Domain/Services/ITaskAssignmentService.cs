using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Services;

/// <summary>
/// Interfaccia per il servizio di assegnazione task
/// </summary>
public interface ITaskAssignmentService : IDomainService
{
    /// <summary>
    /// Assegna un task a un utente
    /// </summary>
    /// <param name="task">Il task da assegnare</param>
    /// <param name="user">L'utente a cui assegnare il task</param>
    /// <returns>True se l'assegnazione è avvenuta con successo, false altrimenti</returns>
    bool AssignTask(DomainTask task, User user);

    /// <summary>
    /// Rimuove l'assegnazione di un task
    /// </summary>
    /// <param name="task">Il task di cui rimuovere l'assegnazione</param>
    /// <returns>True se la rimozione è avvenuta con successo, false altrimenti</returns>
    bool UnassignTask(DomainTask task);

    /// <summary>
    /// Verifica se un utente può essere assegnato a un task
    /// </summary>
    /// <param name="task">Il task</param>
    /// <param name="user">L'utente</param>
    /// <returns>True se l'utente può essere assegnato al task, false altrimenti</returns>
    bool CanAssignUserToTask(DomainTask task, User user);

    /// <summary>
    /// Verifica se un utente è assegnato a un task
    /// </summary>
    /// <param name="task">Il task</param>
    /// <param name="user">L'utente</param>
    /// <returns>True se l'utente è assegnato al task, false altrimenti</returns>
    bool IsUserAssignedToTask(DomainTask task, User user);
}
