using TaskManager.Application.Queries.Projects.GetProjects;
using Xunit;

namespace TaskManager.UnitTests.Queries.Projects.GetProjects;

/// <summary>
/// Unit tests for GetProjectsQuery.
/// </summary>
public class GetProjectsQueryTests
{
    /// <summary>
    /// Ensures that GetProjectsQuery can be instantiated.
    /// </summary>
    [Fact]
    public void GetProjectsQuery_CanBeInstantiated()
    {
        // Act
        var query = new GetProjectsQuery();

        // Assert
        Assert.NotNull(query);
    }
}
