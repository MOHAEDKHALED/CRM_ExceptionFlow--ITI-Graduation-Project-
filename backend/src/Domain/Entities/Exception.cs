using CRM.Domain.Common;
using CRM.Domain.Common.Events;

namespace CRM.Domain.Entities;

public class Exception : AggregateRoot<int>
{
    public string ProjectId { get; private set; } = string.Empty;
    public string Module { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string? StackTrace { get; private set; }
    public ExceptionStatus Status { get; private set; } = ExceptionStatus.Open;
    public ExceptionPriority Priority { get; private set; } = ExceptionPriority.Medium;
    public int ReportedByUserId { get; private set; }
    public int? AssignedToUserId { get; private set; }
    public DateTime ReportedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? ResolvedAt { get; private set; }
    public string? ResolutionNotes { get; private set; }
    
    // Navigation properties (for EF Core)
    public User? ReportedByUser { get; private set; }
    public User? AssignedToUser { get; private set; }
    
    private readonly List<AIRecommendation> _aiRecommendations = new();
    public IReadOnlyCollection<AIRecommendation> AIRecommendations => _aiRecommendations.AsReadOnly();
    
    private readonly List<ExceptionHistory> _history = new();
    public IReadOnlyCollection<ExceptionHistory> History => _history.AsReadOnly();
    
    private Exception() { }
    
    public Exception(
        string projectId,
        string module,
        string title,
        string description,
        int reportedByUserId,
        string? stackTrace = null,
        ExceptionPriority priority = ExceptionPriority.Medium,
        int? assignedToUserId = null)
    {
        ProjectId = projectId;
        Module = module;
        Title = title;
        Description = description;
        StackTrace = stackTrace;
        Priority = priority;
        ReportedByUserId = reportedByUserId;
        AssignedToUserId = assignedToUserId;
        Status = ExceptionStatus.Open;
        ReportedAt = DateTime.UtcNow;
        
        AddDomainEvent(new ExceptionCreatedEvent(Id, title, reportedByUserId, priority));
    }
    
    public void Update(
        string projectId,
        string module,
        string title,
        string description,
        string? stackTrace,
        ExceptionPriority priority)
    {
        ProjectId = projectId;
        Module = module;
        Title = title;
        Description = description;
        StackTrace = stackTrace;
        Priority = priority;
    }
    
    public void ChangeStatus(ExceptionStatus newStatus, string? notes = null)
    {
        if (Status != newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;
            
            if (newStatus == ExceptionStatus.Resolved && ResolvedAt == null)
            {
                ResolvedAt = DateTime.UtcNow;
            }
            else if (newStatus != ExceptionStatus.Resolved)
            {
                ResolvedAt = null;
            }
            
            AddHistoryEntry(newStatus, $"Status changed from {oldStatus} to {newStatus}. {notes}");
            AddDomainEvent(new ExceptionStatusChangedEvent(Id, oldStatus, newStatus));
        }
    }
    
    public void Assign(int userId)
    {
        if (AssignedToUserId != userId)
        {
            AssignedToUserId = userId;
            AddHistoryEntry(Status, $"Exception assigned to user {userId}");
        }
    }
    
    public void Resolve(string resolutionNotes)
    {
        ResolutionNotes = resolutionNotes;
        ChangeStatus(ExceptionStatus.Resolved, resolutionNotes);
    }
    
    public void AddRecommendation(AIRecommendation recommendation)
    {
        _aiRecommendations.Add(recommendation);
    }
    
    private void AddHistoryEntry(ExceptionStatus status, string notes)
    {
        _history.Add(new ExceptionHistory
        {
            ExceptionId = Id,
            Status = status,
            Notes = notes,
            ChangedAt = DateTime.UtcNow
        });
    }
}

public enum ExceptionStatus
{
    Open,
    InProgress,
    Resolved,
    Closed,
    Cancelled
}

public enum ExceptionPriority
{
    Low,
    Medium,
    High,
    Critical
}

