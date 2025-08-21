namespace TaskManager.Shared.Responses;

public class RegisterResponse
{
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();
}
