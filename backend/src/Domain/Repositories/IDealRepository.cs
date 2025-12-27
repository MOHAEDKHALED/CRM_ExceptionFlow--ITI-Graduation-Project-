using CRM.Domain.Entities;

namespace CRM.Domain.Repositories;

public interface IDealRepository : IRepository<Deal, int>
{
    Task<IEnumerable<Deal>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Deal>> GetByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Deal>> GetByStageAsync(DealStage stage, CancellationToken cancellationToken = default);
}


