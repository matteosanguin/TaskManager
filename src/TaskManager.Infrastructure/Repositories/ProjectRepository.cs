using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    /// <summary>
    /// Implementazione del repository per i progetti.
    /// </summary>
    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        private new readonly KanboardDbContext _context;

        /// <summary>
        /// Costruttore.
        /// </summary>
        /// <param name="context">Context del database</param>
        public ProjectRepository(KanboardDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Ottiene i progetti di un proprietario specifico.
        /// </summary>
        /// <param name="ownerId">ID del proprietario</param>
        /// <returns>Lista di progetti del proprietario</returns>
        public async Task<IEnumerable<Project>> GetByOwnerIdAsync(Guid ownerId)
        {
            return await _dbSet.Where(p => p.OwnerId == ownerId).ToListAsync();
        }

        /// <summary>
        /// Ottiene un progetto con i suoi task associati.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>Progetto con i task caricati</returns>
        public async Task<Project?> GetWithTasksAsync(Guid projectId)
        {
            return await _dbSet.Where(p => p.Id == projectId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Ottiene i progetti pubblici.
        /// </summary>
        /// <returns>Lista di progetti pubblici</returns>
        public async Task<IEnumerable<Project>> GetPublicProjectsAsync()
        {
            return await _dbSet.Where(p => !p.IsPrivate).ToListAsync();
        }
    }
}
