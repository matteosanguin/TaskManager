using FastEndpoints;
using TaskManager.Application.Contracts;
using TaskManager.Shared.Requests;
using TaskManager.Shared.Responses;

namespace TaskManager.WebApi.Features.Auth;

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IAuthService _authService;

    public LoginEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var response = await _authService.LoginAsync(req.Username, req.Password);
        if (response.Succeeded)
        {
            await HttpContext.Response.SendAsync(response, 200, cancellation: ct);
        }
        else
        {
            await HttpContext.Response.SendAsync(response, 400, cancellation: ct);
        }
    }
}
