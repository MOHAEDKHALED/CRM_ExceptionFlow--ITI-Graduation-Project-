using CRM.Domain.Entities;

namespace CRM.Domain.Repositories;

public interface IInteractionRepository : IRepository<Interaction, int>
{
    Task<IEnumerable<Interaction>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Interaction>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default);
}


