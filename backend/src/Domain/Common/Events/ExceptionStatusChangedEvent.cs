using CRM.Domain.Entities;

namespace CRM.Domain.Common.Events;

public record ExceptionStatusChangedEvent(
    int ExceptionId,
    ExceptionStatus OldStatus,
    ExceptionStatus NewStatus) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}


