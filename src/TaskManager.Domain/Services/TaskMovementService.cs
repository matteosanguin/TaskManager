using TaskManager.Domain.Entities;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.Domain.Services;

/// <summary>
/// Implementazione del servizio di movimento dei task
/// </summary>
public class TaskMovementService : ITaskMovementService
{
    /// <summary>
    /// Sposta un task da una colonna all'altra
    /// </summary>
    /// <param name="task">Il task da spostare</param>
    /// <param name="targetColumn">La colonna di destinazione</param>
    /// <returns>True se lo spostamento è avvenuto con successo, false altrimenti</returns>
    public bool MoveTask(DomainTask task, Column targetColumn)
    {
        // Verifica se lo spostamento è possibile
        if (!CanMoveTask(task, targetColumn))
        {
            return false;
        }

        // Salva la colonna originale per eventuali rollback
        var originalColumnId = task.ColumnId;

        try
        {
            // Aggiorna la colonna del task
            task.ColumnId = targetColumn.Id;

            // Calcola la nuova posizione (alla fine della colonna)
            task.Position = targetColumn.Tasks.Count + 1;

            // Aggiorna la data di modifica
            task.ModifiedAt = DateTime.UtcNow;

            // Registra il movimento nella history (implementazione futura)
            // RegisterMovementInHistory(task, originalColumnId, targetColumn.Id);

            return true;
        }
        catch
        {
            // In caso di errore, ripristina lo stato originale
            task.ColumnId = originalColumnId;
            return false;
        }
    }

    /// <summary>
    /// Sposta un task da una colonna all'altra in una posizione specifica
    /// </summary>
    /// <param name="task">Il task da spostare</param>
    /// <param name="targetColumn">La colonna di destinazione</param>
    /// <param name="position">La posizione nella colonna di destinazione</param>
    /// <returns>True se lo spostamento è avvenuto con successo, false altrimenti</returns>
    public bool MoveTask(DomainTask task, Column targetColumn, int position)
    {
        // Verifica se lo spostamento è possibile
        if (!CanMoveTask(task, targetColumn))
        {
            return false;
        }

        // Verifica che la posizione sia valida
        if (position < 1 || position > targetColumn.Tasks.Count + 1)
        {
            return false;
        }

        // Salva la colonna originale per eventuali rollback
        var originalColumnId = task.ColumnId;
        var originalPosition = task.Position;

        try
        {
            // Aggiorna la colonna del task
            task.ColumnId = targetColumn.Id;

            // Aggiorna la posizione
            task.Position = position;

            // Aggiorna la data di modifica
            task.ModifiedAt = DateTime.UtcNow;

            // Riordina le posizioni nella colonna di destinazione
            ReorderTasksInColumn(targetColumn, task);

            // Registra il movimento nella history (implementazione futura)
            // RegisterMovementInHistory(task, originalColumnId, targetColumn.Id);

            return true;
        }
        catch
        {
            // In caso di errore, ripristina lo stato originale
            task.ColumnId = originalColumnId;
            task.Position = originalPosition;
            return false;
        }
    }

    /// <summary>
    /// Verifica se è possibile spostare un task in una colonna
    /// </summary>
    /// <param name="task">Il task da spostare</param>
    /// <param name="targetColumn">La colonna di destinazione</param>
    /// <returns>True se lo spostamento è possibile, false altrimenti</returns>
    public bool CanMoveTask(DomainTask task, Column targetColumn)
    {
        // Verifica che il task e la colonna non siano null
        if (task == null || targetColumn == null)
        {
            return false;
        }

        // Verifica che il task appartenga allo stesso progetto della colonna
        if (task.ProjectId != targetColumn.ProjectId)
        {
            return false;
        }

        // Verifica che il task non sia già nella colonna di destinazione
        if (task.ColumnId == targetColumn.Id)
        {
            return false;
        }

        // Verifica il limite di task della colonna se impostato
        if (
            targetColumn.TaskLimit.HasValue
            && targetColumn.Tasks.Count >= targetColumn.TaskLimit.Value
        )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Riordina le posizioni dei task in una colonna dopo uno spostamento
    /// </summary>
    /// <param name="column">La colonna da riordinare</param>
    /// <param name="movedTask">Il task che è stato spostato</param>
    private void ReorderTasksInColumn(Column column, DomainTask movedTask)
    {
        // Ordina i task per posizione
        var orderedTasks = column.Tasks.OrderBy(t => t.Position).ToList();

        // Assegna nuove posizioni sequenziali
        for (int i = 0; i < orderedTasks.Count; i++)
        {
            // Se è il task spostato, salta l'aggiornamento della posizione
            // perché è già stata impostata nel metodo MoveTask
            if (orderedTasks[i].Id != movedTask.Id)
            {
                orderedTasks[i].Position = i + 1;
            }
        }
    }
}
