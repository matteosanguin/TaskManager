using System.Net;
using System.Net.Http.Json;
using TaskManager.Application.Commands.Projects.CreateProject;
using TaskManager.Shared.Responses;
using Xunit;

namespace TaskManager.IntegrationTests.Endpoints.Projects;

/// <summary>
/// Integration tests for GetProjectsEndpoint.
/// </summary>
public class GetProjectsEndpointTests : FastEndpointsTestBase
{
    /// <summary>
    /// Tests that getting all projects returns a 200 OK status and a list of projects.
    /// </summary>
    [Fact]
    public async Task GetProjects_ReturnsOkAndListOfProjects()
    {
        // Arrange
        var client = await GetHttpClientAsync();

        // Create a project first to ensure there's data to retrieve
        var createCommand = new CreateProjectCommand(
            "Test Project for Get",
            "Test project description for Get"
        );
        await client.PostAsJsonAsync("/projects", createCommand);

        // Act
        var response = await client.GetAsync("/projects");

        // Log the response content for debugging
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"GetProjects Response Status: {response.StatusCode}");
        Console.WriteLine($"GetProjects Response Content: {responseContent}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var projects = await response.Content.ReadFromJsonAsync<IEnumerable<ProjectResponse>>();
        Assert.NotNull(projects);
        Assert.NotEmpty(projects);
        Assert.Contains(projects, p => p.Name == "Test Project for Get");
    }
}
