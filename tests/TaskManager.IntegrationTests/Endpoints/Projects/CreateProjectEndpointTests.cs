using System.Net;
using System.Net.Http.Json;
using TaskManager.Application.Commands.Projects.CreateProject;
using TaskManager.Shared.Responses;
using Xunit;

namespace TaskManager.IntegrationTests.Endpoints.Projects;

/// <summary>
/// Integration tests for CreateProjectEndpoint.
/// </summary>
public class CreateProjectEndpointTests : FastEndpointsTestBase
{
    /// <summary>
    /// Tests that a valid project creation request returns a 201 Created status.
    /// </summary>
    [Fact]
    public async Task CreateProject_ValidRequest_ReturnsCreated()
    {
        // Arrange
        var client = await GetHttpClientAsync();
        var command = new CreateProjectCommand("Test Project", "Test project description");

        // Act
        var response = await client.PostAsJsonAsync("/projects", command);

        // Log the response content for debugging
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"CreateProject Response Status: {response.StatusCode}");
        Console.WriteLine($"CreateProject Response Content: {responseContent}");

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var projectResponse = await response.Content.ReadFromJsonAsync<ProjectResponse>();
        Assert.NotNull(projectResponse);
        Assert.Equal("Test Project", projectResponse.Name);
        Assert.Equal("Test project description", projectResponse.Description);
    }

    /// <summary>
    /// Tests that a project creation request with invalid data returns a 400 Bad Request status.
    /// </summary>
    [Fact]
    public async Task CreateProject_InvalidRequest_ReturnsBadRequest()
    {
        // Arrange
        var client = await GetHttpClientAsync();
        var command = new CreateProjectCommand("", "Test project description"); // Empty name

        // Act
        var response = await client.PostAsJsonAsync("/projects", command);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
