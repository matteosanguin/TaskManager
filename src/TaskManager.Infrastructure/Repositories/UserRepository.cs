using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    /// <summary>
    /// Implementazione del repository per gli utenti.
    /// </summary>
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private new readonly KanboardDbContext _context;

        /// <summary>
        /// Costruttore.
        /// </summary>
        /// <param name="context">Context del database</param>
        public UserRepository(KanboardDbContext context)
            : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Ottiene un utente per nome utente.
        /// </summary>
        /// <param name="username">Nome utente</param>
        /// <returns>L'utente trovato o null se non esiste</returns>
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Username == username);
        }

        /// <summary>
        /// Ottiene un utente per email.
        /// </summary>
        /// <param name="email">Email dell'utente</param>
        /// <returns>L'utente trovato o null se non esiste</returns>
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email.Value == email);
        }

        /// <summary>
        /// Ottiene gli utenti attivi.
        /// </summary>
        /// <returns>Lista di utenti attivi</returns>
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.IsActive).ToListAsync();
        }
    }
}
