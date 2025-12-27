using CRM.Domain.Common;
using CRM.Domain.Common.Events;

namespace CRM.Domain.Entities;

public class Customer : AggregateRoot<int>
{
    public string Name { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string? Phone { get; private set; }
    public string? Company { get; private set; }
    public string? Address { get; private set; }
    public CustomerStatus Status { get; private set; }
    public int AssignedToUserId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    
    // Navigation properties (for EF Core)
    public User? AssignedToUser { get; private set; }
    
    private readonly List<Deal> _deals = new();
    public IReadOnlyCollection<Deal> Deals => _deals.AsReadOnly();
    
    private readonly List<Interaction> _interactions = new();
    public IReadOnlyCollection<Interaction> Interactions => _interactions.AsReadOnly();
    
    private Customer() { }
    
    public Customer(
        string name,
        int assignedToUserId,
        string? email = null,
        string? phone = null,
        string? company = null,
        string? address = null)
    {
        Name = name;
        AssignedToUserId = assignedToUserId;
        Email = email;
        Phone = phone;
        Company = company;
        Address = address;
        Status = CustomerStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        
        AddDomainEvent(new CustomerCreatedEvent(Id, name, assignedToUserId));
    }
    
    public void Update(
        string name,
        string? email,
        string? phone,
        string? company,
        string? address)
    {
        Name = name;
        Email = email;
        Phone = phone;
        Company = company;
        Address = address;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ChangeStatus(CustomerStatus status)
    {
        if (Status != status)
        {
            Status = status;
            UpdatedAt = DateTime.UtcNow;
        }
    }
    
    public void Reassign(int userId)
    {
        AssignedToUserId = userId;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum CustomerStatus
{
    Active,
    Inactive,
    Archived
}

