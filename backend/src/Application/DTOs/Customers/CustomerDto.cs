namespace CRM.Application.DTOs.Customers;

public class CustomerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public string Status { get; set; } = string.Empty;
    public int AssignedToUserId { get; set; }
    public string? AssignedToUserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CustomerSummaryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? AssignedToUserName { get; set; }
}

public class CreateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public int AssignedToUserId { get; set; }
}

public class UpdateCustomerRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Company { get; set; }
    public string? Address { get; set; }
    public string? Status { get; set; }
    public int? AssignedToUserId { get; set; }
}


