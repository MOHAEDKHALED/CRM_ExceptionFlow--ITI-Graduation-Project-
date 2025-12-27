namespace CRM.Domain.Common.Events;

public record CustomerCreatedEvent(int CustomerId, string Name, int AssignedToUserId) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}


