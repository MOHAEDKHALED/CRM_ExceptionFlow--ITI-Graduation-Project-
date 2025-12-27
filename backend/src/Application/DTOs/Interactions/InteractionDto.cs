namespace CRM.Application.DTOs.Interactions;

public class InteractionDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime InteractionDate { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int UserId { get; set; }
    public string? UserName { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateInteractionRequest
{
    public string Type { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime? InteractionDate { get; set; }
    public int CustomerId { get; set; }
    public int UserId { get; set; }
}


