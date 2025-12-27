using CRM.Domain.Common;

namespace CRM.Domain.Entities;

public class Interaction : Entity<int>
{
    public InteractionType Type { get; private set; }
    public string Subject { get; private set; } = string.Empty;
    public string? Notes { get; private set; }
    public DateTime InteractionDate { get; private set; } = DateTime.UtcNow;
    public int CustomerId { get; private set; }
    public int UserId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    
    // Navigation properties (for EF Core)
    public Customer? Customer { get; private set; }
    public User? User { get; private set; }
    
    private Interaction() { }
    
    public Interaction(
        InteractionType type,
        string subject,
        int customerId,
        int userId,
        string? notes = null,
        DateTime? interactionDate = null)
    {
        Type = type;
        Subject = subject;
        CustomerId = customerId;
        UserId = userId;
        Notes = notes;
        InteractionDate = interactionDate ?? DateTime.UtcNow;
        CreatedAt = DateTime.UtcNow;
    }
    
    public void Update(string subject, string? notes, DateTime? interactionDate)
    {
        Subject = subject;
        Notes = notes;
        if (interactionDate.HasValue)
            InteractionDate = interactionDate.Value;
    }
}

public enum InteractionType
{
    Call,
    Email,
    Meeting,
    Note,
    Task
}

