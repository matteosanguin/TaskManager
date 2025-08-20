using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    /// <summary>
    /// Interfaccia per il repository degli utenti.
    /// </summary>
    public interface IUserRepository : IRepository<User>
    {
        /// <summary>
        /// Ottiene un utente per nome utente.
        /// </summary>
        /// <param name="username">Nome utente</param>
        /// <returns>L'utente trovato o null se non esiste</returns>
        Task<User?> GetByUsernameAsync(string username);

        /// <summary>
        /// Ottiene un utente per email.
        /// </summary>
        /// <param name="email">Email dell'utente</param>
        /// <returns>L'utente trovato o null se non esiste</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Ottiene gli utenti attivi.
        /// </summary>
        /// <returns>Lista di utenti attivi</returns>
        Task<IEnumerable<User>> GetActiveUsersAsync();
    }
}
