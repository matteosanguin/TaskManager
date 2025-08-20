using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Services;

/// <summary>
/// Implementazione del servizio di controllo permessi sui progetti
/// </summary>
public class ProjectPermissionService : IProjectPermissionService
{
    /// <summary>
    /// Verifica se un utente ha accesso in lettura a un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente ha accesso in lettura, false altrimenti</returns>
    public bool CanReadProject(User user, Project project)
    {
        // Verifica che l'utente e il progetto non siano null
        if (user == null || project == null)
        {
            return false;
        }

        // Il proprietario del progetto ha sempre accesso in lettura
        if (IsProjectOwner(user, project))
        {
            return true;
        }

        // Per ora, implementiamo una logica semplice:
        // - Se il progetto è pubblico, tutti possono leggerlo
        // - Se il progetto è privato, solo il proprietario può leggerlo
        return !project.IsPrivate;
    }

    /// <summary>
    /// Verifica se un utente ha accesso in scrittura a un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente ha accesso in scrittura, false altrimenti</returns>
    public bool CanWriteProject(User user, Project project)
    {
        // Verifica che l'utente e il progetto non siano null
        if (user == null || project == null)
        {
            return false;
        }

        // Solo il proprietario del progetto ha accesso in scrittura
        return IsProjectOwner(user, project);
    }

    /// <summary>
    /// Verifica se un utente è il proprietario di un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente è il proprietario, false altrimenti</returns>
    public bool IsProjectOwner(User user, Project project)
    {
        // Verifica che l'utente e il progetto non siano null
        if (user == null || project == null)
        {
            return false;
        }

        // Un utente è il proprietario di un progetto se il suo ID corrisponde all'OwnerId del progetto
        return user.Id == project.OwnerId;
    }

    /// <summary>
    /// Verifica se un utente può eliminare un progetto
    /// </summary>
    /// <param name="user">L'utente</param>
    /// <param name="project">Il progetto</param>
    /// <returns>True se l'utente può eliminare il progetto, false altrimenti</returns>
    public bool CanDeleteProject(User user, Project project)
    {
        // Per ora, solo il proprietario può eliminare un progetto
        return IsProjectOwner(user, project);
    }
}
