namespace CRM.Application.DTOs.Deals;

public class DealDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Stage { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public int AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateDealRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string Priority { get; set; } = "Medium";
    public int CustomerId { get; set; }
    public int AssignedToUserId { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
}

public class UpdateDealRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public string? Stage { get; set; }
    public string Priority { get; set; } = string.Empty;
    public int? AssignedToUserId { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
}


