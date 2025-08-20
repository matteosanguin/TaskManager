using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Repositories
{
    /// <summary>
    /// Interfaccia per il repository delle board.
    /// </summary>
    public interface IBoardRepository : IRepository<Board>
    {
        /// <summary>
        /// Ottiene una board per ID progetto.
        /// </summary>
        /// <param name="projectId">ID del progetto</param>
        /// <returns>La board associata al progetto o null se non esiste</returns>
        Task<Board?> GetByProjectIdAsync(Guid projectId);

        /// <summary>
        /// Ottiene una board con le sue colonne.
        /// </summary>
        /// <param name="boardId">ID della board</param>
        /// <returns>Board con le colonne caricate</returns>
        Task<Board?> GetWithColumnsAsync(Guid boardId);
    }
}
