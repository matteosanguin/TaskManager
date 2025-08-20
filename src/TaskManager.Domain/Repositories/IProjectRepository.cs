using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    /// <summary>
    /// Interfaccia per il repository dei progetti.
    /// </summary>
    public interface IProjectRepository : IRepository<Project>
    {
        /// <summary>
        /// Ottiene i progetti di un proprietario specifico.
        /// </summary>
        /// <param name="ownerId">ID del proprietario</param>
        /// <returns>Lista di progetti del proprietario</returns>
        Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId);

        /// <summary>
        /// Ottiene un progetto con i suoi task associati.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>Progetto con i task caricati</returns>
        Task<Project?> GetWithTasksAsync(Guid projectId);

        /// <summary>
        /// Ottiene i progetti pubblici.
        /// </summary>
        /// <returns>Lista di progetti pubblici</returns>
        Task<IEnumerable<Project>> GetPublicProjectsAsync();
    }
}
