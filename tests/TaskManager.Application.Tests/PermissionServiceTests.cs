using TaskManager.Application.Contracts;
using TaskManager.Infrastructure.Services;

namespace TaskManager.Application.Tests;

public class PermissionServiceTests
{
    private readonly IPermissionService _permissionService;

    public PermissionServiceTests()
    {
        _permissionService = new PermissionService();
    }

    [Fact]
    public async Task CanAccessProjectAsync_ReturnsTrue()
    {
        // Arrange
        var userId = 1;
        var projectId = 1;

        // Act
        var result = await _permissionService.CanAccessProjectAsync(userId, projectId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CanEditTaskAsync_ReturnsTrue()
    {
        // Arrange
        var userId = 1;
        var taskId = 1;

        // Act
        var result = await _permissionService.CanEditTaskAsync(userId, taskId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task IsProjectOwnerAsync_ReturnsTrue()
    {
        // Arrange
        var userId = 1;
        var projectId = 1;

        // Act
        var result = await _permissionService.IsProjectOwnerAsync(userId, projectId);

        // Assert
        Assert.True(result);
    }
}
