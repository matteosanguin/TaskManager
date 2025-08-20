using Moq;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Services;
using Xunit;
using DomainTask = TaskManager.Domain.Entities.Task;

namespace TaskManager.UnitTests.Services;

public class TaskAssignmentServiceTests
{
    private readonly Mock<IProjectPermissionService> _mockProjectPermissionService;
    private readonly TaskAssignmentService _service;

    public TaskAssignmentServiceTests()
    {
        _mockProjectPermissionService = new Mock<IProjectPermissionService>();
        _service = new TaskAssignmentService(_mockProjectPermissionService.Object);
    }

    [Fact]
    public void AssignTask_ShouldReturnFalse_WhenCannotAssignUserToTask()
    {
        // Arrange
        DomainTask task = new DomainTask();
        User user = new User();

        // Configura il mock per restituire false
        _mockProjectPermissionService
            .Setup(x => x.CanReadProject(user, task.Project))
            .Returns(false);

        // Act
        var result = _service.AssignTask(task, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void AssignTask_ShouldReturnTrue_WhenTaskIsAssignedSuccessfully()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        DomainTask task = new DomainTask
        {
            ProjectId = projectId,
            Project = new Project { Id = projectId }
        };

        User user = new User { Id = userId };

        // Configura il mock per restituire true
        _mockProjectPermissionService
            .Setup(x => x.CanReadProject(user, task.Project))
            .Returns(true);

        // Act
        var result = _service.AssignTask(task, user);

        // Assert
        Assert.True(result);
        Assert.Equal(userId, task.AssigneeId);
        Assert.Equal(user, task.Assignee);
        Assert.True(task.ModifiedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void UnassignTask_ShouldReturnTrue_WhenAssignmentIsRemoved()
    {
        // Arrange
        var userId = Guid.NewGuid();

        DomainTask task = new DomainTask
        {
            AssigneeId = userId,
            Assignee = new User { Id = userId }
        };

        // Act
        var result = _service.UnassignTask(task);

        // Assert
        Assert.True(result);
        Assert.Null(task.AssigneeId);
        Assert.Null(task.Assignee);
        Assert.True(task.ModifiedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void CanAssignUserToTask_ShouldReturnFalse_WhenTaskIsNull()
    {
        // Arrange
        DomainTask task = null!;
        User user = new User();

        // Act
        var result = _service.CanAssignUserToTask(task, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanAssignUserToTask_ShouldReturnFalse_WhenUserIsNull()
    {
        // Arrange
        DomainTask task = new DomainTask();
        User user = null!;

        // Act
        var result = _service.CanAssignUserToTask(task, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanAssignUserToTask_ShouldReturnFalse_WhenUserCannotReadProject()
    {
        // Arrange
        var projectId = Guid.NewGuid();

        DomainTask task = new DomainTask
        {
            ProjectId = projectId,
            Project = new Project { Id = projectId }
        };

        User user = new User();

        // Configura il mock per restituire false
        _mockProjectPermissionService
            .Setup(x => x.CanReadProject(user, task.Project))
            .Returns(false);

        // Act
        var result = _service.CanAssignUserToTask(task, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanAssignUserToTask_ShouldReturnFalse_WhenUserIsAlreadyAssigned()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        DomainTask task = new DomainTask
        {
            ProjectId = projectId,
            Project = new Project { Id = projectId },
            AssigneeId = userId
        };

        User user = new User { Id = userId };

        // Configura il mock per restituire true
        _mockProjectPermissionService
            .Setup(x => x.CanReadProject(user, task.Project))
            .Returns(true);

        // Act
        var result = _service.CanAssignUserToTask(task, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanAssignUserToTask_ShouldReturnTrue_WhenAllConditionsAreMet()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        DomainTask task = new DomainTask
        {
            ProjectId = projectId,
            Project = new Project { Id = projectId },
            AssigneeId = userId1
        };

        User user = new User { Id = userId2 };

        // Configura il mock per restituire true
        _mockProjectPermissionService
            .Setup(x => x.CanReadProject(user, task.Project))
            .Returns(true);

        // Act
        var result = _service.CanAssignUserToTask(task, user);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUserAssignedToTask_ShouldReturnFalse_WhenTaskIsNull()
    {
        // Arrange
        DomainTask task = null!;
        User user = new User();

        // Act
        var result = _service.IsUserAssignedToTask(task, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsUserAssignedToTask_ShouldReturnFalse_WhenUserIsNull()
    {
        // Arrange
        DomainTask task = new DomainTask();
        User user = null!;

        // Act
        var result = _service.IsUserAssignedToTask(task, user);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsUserAssignedToTask_ShouldReturnTrue_WhenUserIsAssigned()
    {
        // Arrange
        var userId = Guid.NewGuid();

        DomainTask task = new DomainTask { AssigneeId = userId };

        User user = new User { Id = userId };

        // Act
        var result = _service.IsUserAssignedToTask(task, user);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsUserAssignedToTask_ShouldReturnFalse_WhenUserIsNotAssigned()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        DomainTask task = new DomainTask { AssigneeId = userId1 };

        User user = new User { Id = userId2 };

        // Act
        var result = _service.IsUserAssignedToTask(task, user);

        // Assert
        Assert.False(result);
    }
}
