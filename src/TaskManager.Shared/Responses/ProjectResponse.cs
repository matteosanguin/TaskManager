namespace TaskManager.Shared.Responses;

public record ProjectResponse(Guid Id, string Name, string? Description, DateTime CreatedAt);
