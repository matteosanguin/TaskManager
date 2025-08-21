using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskManager.Application.Contracts;
using TaskManager.Infrastructure.Identity;
using TaskManager.Shared.Requests;
using TaskManager.Shared.Responses;

namespace TaskManager.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<LoginResponse> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
        {
            return new LoginResponse { Succeeded = false, Error = "Invalid username or password" };
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
        {
            return new LoginResponse { Succeeded = false, Error = "Invalid username or password" };
        }

        var token = GenerateJwtToken(user);
        return new LoginResponse { Succeeded = true, Token = token };
    }

    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        var user = new AppUser
        {
            UserName = request.Username,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return new RegisterResponse { Succeeded = false, Errors = result.Errors.Select(e => e.Description).ToArray() };
        }

        return new RegisterResponse { Succeeded = true };
    }

    public Task<UserResponse> GetCurrentUserAsync()
    {
        // This will be implemented later
        throw new NotImplementedException();
    }

    public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
    {
        var user = await _userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            return new UpdateUserResponse { Succeeded = false, Errors = new[] { "User not found" } };
        }

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return new UpdateUserResponse { Succeeded = false, Errors = result.Errors.Select(e => e.Description).ToArray() };
        }

        return new UpdateUserResponse { Succeeded = true };
    }

    private string GenerateJwtToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Issuer"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
