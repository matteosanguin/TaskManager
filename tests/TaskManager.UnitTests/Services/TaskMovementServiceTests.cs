using TaskManager.Domain.Entities;
using TaskManager.Domain.Services;
using Xunit;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.UnitTests.Services;

public class TaskMovementServiceTests
{
    private readonly TaskMovementService _service;

    public TaskMovementServiceTests()
    {
        _service = new TaskMovementService();
    }

    [Fact]
    public void CanMoveTask_ShouldReturnFalse_WhenTaskIsNull()
    {
        // Arrange
        DomainTask task = null!;
        Column targetColumn = new Column();

        // Act
        var result = _service.CanMoveTask(task, targetColumn);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanMoveTask_ShouldReturnFalse_WhenColumnIsNull()
    {
        // Arrange
        DomainTask task = new DomainTask();
        Column targetColumn = null!;

        // Act
        var result = _service.CanMoveTask(task, targetColumn);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanMoveTask_ShouldReturnFalse_WhenTaskAndColumnAreInDifferentProjects()
    {
        // Arrange
        var projectId1 = Guid.NewGuid();
        var projectId2 = Guid.NewGuid();

        DomainTask task = new DomainTask { ProjectId = projectId1 };

        Column targetColumn = new Column { ProjectId = projectId2 };

        // Act
        var result = _service.CanMoveTask(task, targetColumn);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanMoveTask_ShouldReturnFalse_WhenTaskIsAlreadyInTargetColumn()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var columnId = Guid.NewGuid();

        DomainTask task = new DomainTask { ProjectId = projectId, ColumnId = columnId };

        Column targetColumn = new Column { Id = columnId, ProjectId = projectId };

        // Act
        var result = _service.CanMoveTask(task, targetColumn);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanMoveTask_ShouldReturnFalse_WhenTargetColumnHasReachedTaskLimit()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var columnId1 = Guid.NewGuid();
        var columnId2 = Guid.NewGuid();

        DomainTask task = new DomainTask { ProjectId = projectId, ColumnId = columnId1 };

        Column targetColumn = new Column
        {
            Id = columnId2,
            ProjectId = projectId,
            TaskLimit = 1,
            Tasks = new List<DomainTask> { new DomainTask() } // La colonna ha già un task
        };

        // Act
        var result = _service.CanMoveTask(task, targetColumn);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanMoveTask_ShouldReturnTrue_WhenAllConditionsAreMet()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var columnId1 = Guid.NewGuid();
        var columnId2 = Guid.NewGuid();

        DomainTask task = new DomainTask { ProjectId = projectId, ColumnId = columnId1 };

        Column targetColumn = new Column
        {
            Id = columnId2,
            ProjectId = projectId,
            TaskLimit = 2,
            Tasks = new List<DomainTask> { new DomainTask() } // La colonna ha un task, il limite è 2
        };

        // Act
        var result = _service.CanMoveTask(task, targetColumn);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void MoveTask_ShouldReturnFalse_WhenCannotMoveTask()
    {
        // Arrange
        DomainTask task = new DomainTask();
        Column targetColumn = new Column();

        // Act
        var result = _service.MoveTask(task, targetColumn);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void MoveTask_ShouldReturnTrue_WhenTaskIsMovedSuccessfully()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var columnId1 = Guid.NewGuid();
        var columnId2 = Guid.NewGuid();

        DomainTask task = new DomainTask
        {
            Id = Guid.NewGuid(),
            ProjectId = projectId,
            ColumnId = columnId1,
            Position = 1
        };

        Column targetColumn = new Column
        {
            Id = columnId2,
            ProjectId = projectId,
            Tasks = new List<DomainTask>()
        };

        // Act
        var result = _service.MoveTask(task, targetColumn);

        // Assert
        Assert.True(result);
        Assert.Equal(columnId2, task.ColumnId);
        Assert.Equal(1, task.Position); // Posizione alla fine della colonna vuota
        Assert.True(task.ModifiedAt <= DateTime.UtcNow);
    }
}
