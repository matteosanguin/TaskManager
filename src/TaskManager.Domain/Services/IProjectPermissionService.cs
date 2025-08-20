using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Services;

/// <summary>
/// Interfaccia per il servizio di controllo permessi sui progetti
/// </summary>
public interface IProjectPermissionService : IDomainService
{
    /// <summary>
    /// Verifica se un utente ha accesso in lettura a un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente ha accesso in lettura, false altrimenti</returns>
    bool CanReadProject(User user, Project project);

    /// <summary>
    /// Verifica se un utente ha accesso in scrittura a un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente ha accesso in scrittura, false altrimenti</returns>
    bool CanWriteProject(User user, Project project);

    /// <summary>
    /// Verifica se un utente è il proprietario di un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente è il proprietario, false altrimenti</returns>
    bool IsProjectOwner(User user, Project project);

    /// <summary>
    /// Verifica se un utente può eliminare un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente può eliminare il progetto, false altrimenti</returns>
    bool CanDeleteProject(User user, Project project);
}
