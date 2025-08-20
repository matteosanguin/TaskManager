using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    /// <summary>
    /// Implementazione del repository per i task.
    /// </summary>
    public class TaskRepository : BaseRepository<Domain.Entities.Task>, ITaskRepository
    {
        private new readonly KanboardDbContext _context;

        /// <summary>
        /// Costruttore.
        /// </summary>
        /// <param name="context">Context del database</param>
        public TaskRepository(KanboardDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Ottiene i task di una colonna specifica.
        /// </summary>
        /// <param name="columnId">ID della colonna</param>
        /// <returns>Lista di task nella colonna</returns>
        public async Task<IEnumerable<Domain.Entities.Task>> GetByColumnIdAsync(Guid columnId)
        {
            return await _dbSet.Where(t => t.ColumnId == columnId).ToListAsync();
        }

        /// <summary>
        /// Ottiene i task assegnati a un utente specifico.
        /// </summary>
        /// <param name="assigneeId">ID dell'utente assegnatario</param>
        /// <returns>Lista di task assegnati all'utente</returns>
        public async Task<IEnumerable<Domain.Entities.Task>> GetByAssigneeIdAsync(Guid assigneeId)
        {
            return await _dbSet.Where(t => t.AssigneeId == assigneeId).ToListAsync();
        }

        /// <summary>
        /// Cerca task per una query di testo.
        /// </summary>
        /// <param name="query">Testo da cercare</param>
        /// <returns>Lista di task che corrispondono alla query</returns>
        public async Task<IEnumerable<Domain.Entities.Task>> SearchTasksAsync(string query)
        {
            return await _dbSet
                .Where(t => t.Title.Contains(query) || t.Description.Contains(query))
                .ToListAsync();
        }

        /// <summary>
        /// Ottiene la cronologia di un task specifico.
        /// </summary>
        /// <param name="taskId">ID del task</param>
        /// <returns>Cronologia del task</returns>
        public async Task<IEnumerable<Domain.Entities.Task>> GetTaskHistoryAsync(Guid taskId)
        {
            // Per ora restituiamo solo il task stesso, in futuro potremmo implementare
            // una vera cronologia dei cambiamenti
            var task = await _dbSet.FindAsync(taskId);
            return task != null ? new[] { task } : Enumerable.Empty<Domain.Entities.Task>();
        }
    }
}
