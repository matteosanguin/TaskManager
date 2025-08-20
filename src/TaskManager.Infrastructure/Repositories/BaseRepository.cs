using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Common;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    /// <summary>
    /// Implementazione base del repository pattern.
    /// Fornisce operazioni CRUD di base per le entità.
    /// </summary>
    /// <typeparam name="T">Tipo dell'entità</typeparam>
    public class BaseRepository<T> : IRepository<T>
        where T : BaseEntity
    {
        protected readonly KanboardDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Costruttore.
        /// </summary>
        /// <param name="context">Context del database</param>
        public BaseRepository(KanboardDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        /// <summary>
        /// Ottiene un'entità per ID.
        /// </summary>
        /// <param name="id">ID dell'entità</param>
        /// <returns>L'entità trovata o null se non esiste</returns>
        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <summary>
        /// Ottiene tutte le entità.
        /// </summary>
        /// <returns>Lista di tutte le entità</returns>
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <summary>
        /// Ottiene entità filtrate da un predicato.
        /// </summary>
        /// <param name="predicate">Predicato per il filtro</param>
        /// <returns>Lista di entità filtrate</returns>
        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        /// <summary>
        /// Aggiunge una nuova entità.
        /// </summary>
        /// <param name="entity">Entità da aggiungere</param>
        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        /// <summary>
        /// Aggiunge più entità.
        /// </summary>
        /// <param name="entities">Entità da aggiungere</param>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Aggiorna un'entità esistente.
        /// </summary>
        /// <param name="entity">Entità da aggiornare</param>
        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        /// <summary>
        /// Rimuove un'entità.
        /// </summary>
        /// <param name="entity">Entità da rimuovere</param>
        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        /// <summary>
        /// Rimuove più entità.
        /// </summary>
        /// <param name="entities">Entità da rimuovere</param>
        public void RemoveRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }
    }
}
