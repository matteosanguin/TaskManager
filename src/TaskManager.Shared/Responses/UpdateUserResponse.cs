namespace TaskManager.Shared.Responses;

public class UpdateUserResponse
{
    public bool Succeeded { get; set; }
    public string[] Errors { get; set; } = Array.Empty<string>();
}
