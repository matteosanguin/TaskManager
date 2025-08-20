using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    /// <summary>
    /// Implementazione del repository per le board.
    /// </summary>
    public class BoardRepository : BaseRepository<Board>, IBoardRepository
    {
        private new readonly KanboardDbContext _context;

        /// <summary>
        /// Costruttore.
        /// </summary>
        /// <param name="context">Context del database</param>
        public BoardRepository(KanboardDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Ottiene una board per ID progetto.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>La board associata al progetto o null se non esiste</returns>
        public async Task<Board?> GetByProjectIdAsync(Guid projectId)
        {
            return await _dbSet.FirstOrDefaultAsync(b => b.ProjectId == projectId);
        }

        /// <summary>
        /// Ottiene una board con le sue colonne.
        /// </summary>
        /// <param name="boardId">ID della board</param>
        /// <returns>Board con le colonne caricate</returns>
        public async Task<Board?> GetWithColumnsAsync(Guid boardId)
        {
            return await _dbSet.Where(b => b.Id == boardId).FirstOrDefaultAsync();
        }
    }
}
