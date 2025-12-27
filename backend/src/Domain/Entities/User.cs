using CRM.Domain.Common;
using CRM.Domain.ValueObjects;

namespace CRM.Domain.Entities;

public class User : AggregateRoot<int>
{
    public string Username { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public string FullName { get; private set; } = string.Empty;
    public UserRole Role { get; private set; }
    public string? Department { get; private set; }
    public int? TeamId { get; private set; }
    public bool IsActive { get; private set; } = true;
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; private set; }
    
    // Navigation properties
    private readonly List<CRM.Domain.Entities.Exception> _reportedExceptions = new();
    public IReadOnlyCollection<CRM.Domain.Entities.Exception> ReportedExceptions => _reportedExceptions.AsReadOnly();
    
    private readonly List<CRM.Domain.Entities.Exception> _assignedExceptions = new();
    public IReadOnlyCollection<CRM.Domain.Entities.Exception> AssignedExceptions => _assignedExceptions.AsReadOnly();
    
    private readonly List<Customer> _customers = new();
    public IReadOnlyCollection<Customer> Customers => _customers.AsReadOnly();
    
    private readonly List<Deal> _deals = new();
    public IReadOnlyCollection<Deal> Deals => _deals.AsReadOnly();
    
    private readonly List<Interaction> _interactions = new();
    public IReadOnlyCollection<Interaction> Interactions => _interactions.AsReadOnly();
    
    private User() 
    {
        Role = UserRole.Employee; // Default for EF Core
    }
    
    public User(
        string username,
        string email,
        string passwordHash,
        string fullName,
        UserRole role,
        string? department = null,
        int? teamId = null)
    {
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
        FullName = fullName;
        Role = role;
        Department = department;
        TeamId = teamId;
    }
    
    public void UpdateProfile(string fullName, string? department)
    {
        FullName = fullName;
        Department = department;
    }
    
    public void UpdateRole(UserRole role)
    {
        Role = role;
    }
    
    public void Activate()
    {
        IsActive = true;
    }
    
    public void Deactivate()
    {
        IsActive = false;
    }
    
    public void RecordLogin()
    {
        LastLogin = DateTime.UtcNow;
    }
}

