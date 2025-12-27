using CRM.Domain.Common;

namespace CRM.Domain.Entities;

public class Deal : AggregateRoot<int>
{
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public decimal Amount { get; private set; }
    public DealStage Stage { get; private set; } = DealStage.Prospecting;
    public DealPriority Priority { get; private set; } = DealPriority.Medium;
    public int CustomerId { get; private set; }
    public int AssignedToUserId { get; private set; }
    public DateTime? ExpectedCloseDate { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    
    // Navigation properties (for EF Core)
    public Customer? Customer { get; private set; }
    public User? AssignedToUser { get; private set; }
    
    private Deal() { }
    
    public Deal(
        string title,
        decimal amount,
        int customerId,
        int assignedToUserId,
        string? description = null,
        DealPriority priority = DealPriority.Medium,
        DateTime? expectedCloseDate = null)
    {
        Title = title;
        Amount = amount;
        CustomerId = customerId;
        AssignedToUserId = assignedToUserId;
        Description = description;
        Priority = priority;
        ExpectedCloseDate = expectedCloseDate;
        Stage = DealStage.Prospecting;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Update(
        string title,
        string? description,
        decimal amount,
        DealPriority priority,
        DateTime? expectedCloseDate)
    {
        Title = title;
        Description = description;
        Amount = amount;
        Priority = priority;
        ExpectedCloseDate = expectedCloseDate;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ChangeStage(DealStage stage)
    {
        Stage = stage;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void Reassign(int userId)
    {
        AssignedToUserId = userId;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum DealStage
{
    Prospecting,
    Qualification,
    Proposal,
    Negotiation,
    ClosedWon,
    ClosedLost
}

public enum DealPriority
{
    Low,
    Medium,
    High,
    Critical
}

