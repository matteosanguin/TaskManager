using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskManager.Application.Contracts;
using TaskManager.Infrastructure.Identity;
using TaskManager.Infrastructure.Services;
using TaskManager.Shared.Requests;

namespace TaskManager.Application.Tests;

public class AuthServiceTests
{
    private readonly Mock<UserManager<AppUser>> _userManagerMock;
    private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        _userManagerMock = new Mock<UserManager<AppUser>>(Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        _signInManagerMock = new Mock<SignInManager<AppUser>>(_userManagerMock.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(), null, null, null, null);
        _configurationMock = new Mock<IConfiguration>();

        _authService = new AuthService(_userManagerMock.Object, _signInManagerMock.Object, _configurationMock.Object);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ReturnsSuccess()
    {
        // Arrange
        var username = "testuser";
        var password = "password";
        var user = new AppUser { UserName = username };
        _userManagerMock.Setup(x => x.FindByNameAsync(username)).ReturnsAsync(user);
        _signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, password, false)).ReturnsAsync(SignInResult.Success);
        _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Key")]).Returns("super-secret-key-that-is-long-enough");
        _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Issuer")]).Returns("TaskManager");
        _configurationMock.SetupGet(x => x[It.Is<string>(s => s == "Jwt:ExpireDays")]).Returns("1");

        // Act
        var result = await _authService.LoginAsync(username, password);

        // Assert
        Assert.True(result.Succeeded);
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async Task RegisterAsync_WithValidData_ReturnsSuccess()
    {
        // Arrange
        var request = new RegisterRequest
        {
            Username = "testuser",
            Email = "test@test.com",
            Password = "password",
            FirstName = "Test",
            LastName = "User"
        };
        _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<AppUser>(), request.Password)).ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.True(result.Succeeded);
    }
}
