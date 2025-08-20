using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Services;

/// <summary>
/// Interfaccia per il servizio di movimento dei task
/// </summary>
public interface ITaskMovementService : IDomainService
{
    /// <summary>
    /// Sposta un task da una colonna all'altra
    /// </summary>
    /// <param name="task">Il task da spostare</param>
    /// <param name="targetColumn">La colonna di destinazione</param>
    /// <returns>True se lo spostamento è avvenuto con successo, false altrimenti</returns>
    bool MoveTask(DomainTask task, Column targetColumn);

    /// <summary>
    /// Sposta un task da una colonna all'altra in una posizione specifica
    /// </summary>
    /// <param name="task">Il task da spostare</param>
    /// <param name="targetColumn">La colonna di destinazione</param>
    /// <param name="position">La posizione nella colonna di destinazione</param>
    /// <returns>True se lo spostamento è avvenuto con successo, false altrimenti</returns>
    bool MoveTask(DomainTask task, Column targetColumn, int position);

    /// <summary>
    /// Verifica se è possibile spostare un task in una colonna
    /// </summary>
    /// <param name="task">Il task da spostare</param>
    /// <param name="targetColumn">La colonna di destinazione</param>
    /// <returns>True se lo spostamento è possibile, false altrimenti</returns>
    bool CanMoveTask(DomainTask task, Column targetColumn);
}
