using CRM.Domain.Common;

namespace CRM.Domain.Entities;

public class ExceptionHistory : Entity<int>
{
    public int ExceptionId { get; internal set; }
    public ExceptionStatus Status { get; set; }
    public string ChangedByUserName { get; internal set; } = string.Empty;
    public string? Notes { get; internal set; }
    public DateTime ChangedAt { get; internal set; } = DateTime.UtcNow;
    
    // Navigation property (for EF Core)
    public CRM.Domain.Entities.Exception? Exception { get; private set; }
    
    internal ExceptionHistory() { }
    
    public ExceptionHistory(
        int exceptionId,
        ExceptionStatus status,
        string changedByUserName,
        string? notes = null)
    {
        ExceptionId = exceptionId;
        Status = status;
        ChangedByUserName = changedByUserName;
        Notes = notes;
        ChangedAt = DateTime.UtcNow;
    }
}

