using System.Linq.Expressions;
using TaskManager.Domain.Common;

namespace TaskManager.Domain.Repositories
{
    /// <summary>
    /// Interfaccia generica per il repository pattern.
    /// Fornisce operazioni CRUD di base per le entità.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    public interface IRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// Ottiene un'entità per ID.
        /// </summary>
        /// <param name="id">ID dell'entità</param>
        /// <returns>L'entità trovata o null se non esiste</returns>
        Task<T?> GetByIdAsync(Guid id);

        /// <summary>
        /// Ottiene tutte le entità.
        /// </summary>
        /// <returns>Lista di tutte le entità</returns>
        Task<IEnumerable<T>> GetAllAsync();

        /// <summary>
        /// Ottiene entità filtrate da un predicato.
        /// </summary>
        /// <param name="predicate">Predicato per il filtro</param>
        /// <returns>Lista di entità filtrate</returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Aggiunge una nuova entità.
        /// </summary>
        /// <param name="entity">Entità da aggiungere</param>
        Task AddAsync(T entity);

        /// <summary>
        /// Aggiunge più entità.
        /// </summary>
        /// <param name="entities">Entità da aggiungere</param>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Aggiorna un'entità esistente.
        /// </summary>
        /// <param name="entity">Entità da aggiornare</param>
        void Update(T entity);

        /// <summary>
        /// Rimuove un'entità.
        /// </summary>
        /// <param name="entity">Entità da rimuovere</param>
        void Remove(T entity);

        /// <summary>
        /// Rimuove più entità.
        /// </summary>
        /// <param name="entities">Entità da rimuovere</param>
        void RemoveRange(IEnumerable<T> entities);
    }
}
