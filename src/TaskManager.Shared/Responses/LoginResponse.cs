namespace TaskManager.Shared.Responses;

public class LoginResponse
{
    public bool Succeeded { get; set; }
    public string Token { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;
}
