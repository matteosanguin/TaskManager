using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    /// <summary>
    /// Interfaccia per il repository delle colonne.
    /// </summary>
    public interface IColumnRepository : IRepository<Column>
    {
        /// <summary>
        /// Ottiene le colonne di un progetto.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>Lista di colonne del progetto</returns>
        Task<IEnumerable<Column>> GetByProjectIdAsync(Guid projectId);

        /// <summary>
        /// Ottiene le colonne ordinate per posizione.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>Lista di colonne ordinate per posizione</returns>
        Task<IEnumerable<Column>> GetByProjectIdOrderedAsync(Guid projectId);
    }
}
