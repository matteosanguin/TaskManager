using FastEndpoints;
using TaskManager.Application.Contracts;
using TaskManager.Shared.Requests;
using TaskManager.Shared.Responses;

namespace TaskManager.WebApi.Features.Auth;

public class RegisterEndpoint : Endpoint<RegisterRequest, RegisterResponse>
{
    private readonly IAuthService _authService;

    public RegisterEndpoint(IAuthService authService)
    {
        _authService = authService;
    }

    public override void Configure()
    {
        Post("/api/auth/register");
        AllowAnonymous();
    }

    public override async Task HandleAsync(RegisterRequest req, CancellationToken ct)
    {
        var response = await _authService.RegisterAsync(req);
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
