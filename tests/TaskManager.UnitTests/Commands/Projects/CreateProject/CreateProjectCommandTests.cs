using System.ComponentModel.DataAnnotations;
using TaskManager.Application.Commands.Projects.CreateProject;
using TaskManager.Shared.Responses;
using Xunit;

namespace TaskManager.UnitTests.Commands.Projects.CreateProject;

/// <summary>
/// Unit tests for CreateProjectCommand.
/// </summary>
public class CreateProjectCommandTests
{
    /// <summary>
    /// Validates that a valid command passes validation.
    /// </summary>
    [Fact]
    public void Validate_ValidCommand_PassesValidation()
    {
        // Arrange
        var command = new CreateProjectCommand("Valid Project Name", "Valid project description");

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            command,
            new ValidationContext(command),
            validationResults,
            true
        );

        // Assert
        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    /// <summary>
    /// Validates that a command with an empty name fails validation.
    /// </summary>
    [Fact]
    public void Validate_EmptyName_FailsValidation()
    {
        // Arrange
        var command = new CreateProjectCommand("", "Valid project description");

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            command,
            new ValidationContext(command),
            validationResults,
            true
        );

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Name"));
    }

    /// <summary>
    /// Validates that a command with a name exceeding 100 characters fails validation.
    /// </summary>
    [Fact]
    public void Validate_NameTooLong_FailsValidation()
    {
        // Arrange
        var longName = new string('A', 101);
        var command = new CreateProjectCommand(longName, "Valid project description");

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            command,
            new ValidationContext(command),
            validationResults,
            true
        );

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Name"));
    }

    /// <summary>
    /// Validates that a command with a description exceeding 500 characters fails validation.
    /// </summary>
    [Fact]
    public void Validate_DescriptionTooLong_FailsValidation()
    {
        // Arrange
        var longDescription = new string('A', 501);
        var command = new CreateProjectCommand("Valid Project Name", longDescription);

        // Act
        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(
            command,
            new ValidationContext(command),
            validationResults,
            true
        );

        // Assert
        Assert.False(isValid);
        Assert.Contains(validationResults, vr => vr.MemberNames.Contains("Description"));
    }
}
