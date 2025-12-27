using CRM.Domain.Entities;

namespace CRM.Domain.Common.Events;

public record ExceptionCreatedEvent(
    int ExceptionId,
    string Title,
    int ReportedByUserId,
    ExceptionPriority Priority) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}


