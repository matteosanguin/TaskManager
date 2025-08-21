using TaskManager.Shared.Requests;
using TaskManager.Shared.Responses;

namespace TaskManager.Application.Contracts;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(string username, string password);
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    Task<UserResponse> GetCurrentUserAsync();
    Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
}
