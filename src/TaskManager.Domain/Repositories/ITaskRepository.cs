using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    /// <summary>
    /// Interfaccia per il repository dei task.
    /// </summary>
    public interface ITaskRepository : IRepository<Domain.Entities.Task>
    {
        /// <summary>
        /// Ottiene i task di una colonna specifica.
        /// </summary>
        /// <param name="columnId">ID della colonna</param>
        /// <returns>Lista di task nella colonna</returns>
        Task<IEnumerable<Domain.Entities.Task>> GetByColumnIdAsync(Guid columnId);

        /// <summary>
        /// Ottiene i task assegnati a un utente specifico.
        /// </summary>
        /// <param name="assigneeId">ID dell'utente assegnatario</param>
        /// <returns>Lista di task assegnati all'utente</returns>
        Task<IEnumerable<Domain.Entities.Task>> GetByAssigneeIdAsync(Guid assigneeId);

        /// <summary>
        /// Cerca task per una query di testo.
        /// </summary>
        /// <param name="query">Testo da cercare</param>
        /// <returns>Lista di task che corrispondono alla query</returns>
        Task<IEnumerable<Domain.Entities.Task>> SearchTasksAsync(string query);

        /// <summary>
        /// Ottiene la cronologia di un task specifico.
        /// </summary>
        /// <param name="taskId">ID del task</param>
        /// <returns>Cronologia del task</returns>
        Task<IEnumerable<Domain.Entities.Task>> GetTaskHistoryAsync(Guid taskId);
    }
}
