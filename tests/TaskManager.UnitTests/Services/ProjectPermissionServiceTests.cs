using TaskManager.Domain.Entities;
using TaskManager.Domain.Services;
using Xunit;

namespace TaskManager.UnitTests.Services;

public class ProjectPermissionServiceTests
{
    private readonly ProjectPermissionService _service;

    public ProjectPermissionServiceTests()
    {
        _service = new ProjectPermissionService();
    }

    [Fact]
    public void CanReadProject_ShouldReturnFalse_WhenUserIsNull()
    {
        // Arrange
        User user = null!;
        Project project = new Project();

        // Act
        var result = _service.CanReadProject(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanReadProject_ShouldReturnFalse_WhenProjectIsNull()
    {
        // Arrange
        User user = new User();
        Project project = null!;

        // Act
        var result = _service.CanReadProject(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanReadProject_ShouldReturnTrue_WhenUserIsProjectOwner()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId };

        Project project = new Project
        {
            Id = projectId,
            OwnerId = userId,
            IsPrivate = true // Anche se il progetto è privato, il proprietario può leggerlo
        };

        // Act
        var result = _service.CanReadProject(user, project);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanReadProject_ShouldReturnTrue_WhenProjectIsPublic()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId1 };

        Project project = new Project
        {
            Id = projectId,
            OwnerId = userId2,
            IsPrivate = false // Progetto pubblico
        };

        // Act
        var result = _service.CanReadProject(user, project);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanReadProject_ShouldReturnFalse_WhenProjectIsPrivateAndUserIsNotOwner()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId1 };

        Project project = new Project
        {
            Id = projectId,
            OwnerId = userId2,
            IsPrivate = true // Progetto privato
        };

        // Act
        var result = _service.CanReadProject(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanWriteProject_ShouldReturnFalse_WhenUserIsNull()
    {
        // Arrange
        User user = null!;
        Project project = new Project();

        // Act
        var result = _service.CanWriteProject(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanWriteProject_ShouldReturnFalse_WhenProjectIsNull()
    {
        // Arrange
        User user = new User();
        Project project = null!;

        // Act
        var result = _service.CanWriteProject(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanWriteProject_ShouldReturnTrue_WhenUserIsProjectOwner()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId };

        Project project = new Project { Id = projectId, OwnerId = userId };

        // Act
        var result = _service.CanWriteProject(user, project);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanWriteProject_ShouldReturnFalse_WhenUserIsNotProjectOwner()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId1 };

        Project project = new Project { Id = projectId, OwnerId = userId2 };

        // Act
        var result = _service.CanWriteProject(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsProjectOwner_ShouldReturnFalse_WhenUserIsNull()
    {
        // Arrange
        User user = null!;
        Project project = new Project();

        // Act
        var result = _service.IsProjectOwner(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsProjectOwner_ShouldReturnFalse_WhenProjectIsNull()
    {
        // Arrange
        User user = new User();
        Project project = null!;

        // Act
        var result = _service.IsProjectOwner(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsProjectOwner_ShouldReturnTrue_WhenUserIsProjectOwner()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId };

        Project project = new Project { Id = projectId, OwnerId = userId };

        // Act
        var result = _service.IsProjectOwner(user, project);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsProjectOwner_ShouldReturnFalse_WhenUserIsNotProjectOwner()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId1 };

        Project project = new Project { Id = projectId, OwnerId = userId2 };

        // Act
        var result = _service.IsProjectOwner(user, project);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void CanDeleteProject_ShouldReturnTrue_WhenUserIsProjectOwner()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId };

        Project project = new Project { Id = projectId, OwnerId = userId };

        // Act
        var result = _service.CanDeleteProject(user, project);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void CanDeleteProject_ShouldReturnFalse_WhenUserIsNotProjectOwner()
    {
        // Arrange
        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();
        var projectId = Guid.NewGuid();

        User user = new User { Id = userId1 };

        Project project = new Project { Id = projectId, OwnerId = userId2 };

        // Act
        var result = _service.CanDeleteProject(user, project);

        // Assert
        Assert.False(result);
    }
}
