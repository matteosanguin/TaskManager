using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    /// <summary>
    /// Implementazione del repository per le colonne.
    /// </summary>
    public class ColumnRepository : BaseRepository<Column>, IColumnRepository
    {
        private readonly KanboardDbContext _context;

        /// <summary>
        /// Costruttore.
        /// </summary>
        /// <param name="context">Context del database</param>
        public ColumnRepository(KanboardDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Ottiene le colonne di un progetto.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>Lista di colonne del progetto</returns>
        public async Task<IEnumerable<Column>> GetByProjectIdAsync(Guid projectId)
        {
            return await _context.Columns.Where(c => c.ProjectId == projectId).ToListAsync();
        }

        /// <summary>
        /// Ottiene le colonne ordinate per posizione.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>Lista di colonne ordinate per posizione</returns>
        public async Task<IEnumerable<Column>> GetByProjectIdOrderedAsync(Guid projectId)
        {
            return await _context
                .Columns.Where(c => c.ProjectId == projectId)
                .OrderBy(c => c.Position)
                .ToListAsync();
        }
    }
}
