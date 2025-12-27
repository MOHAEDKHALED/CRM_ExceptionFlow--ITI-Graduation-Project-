using CRM.Domain.Entities;
using CRM.Domain.Repositories;
using CRM.Infrastructure.Data;

namespace CRM.Infrastructure.Repositories;

public class DealRepository : BaseRepository<Deal, int>, IDealRepository
{
    public DealRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Deal>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(d => d.CustomerId == customerId, cancellationToken);
    }
    
    public async Task<IEnumerable<Deal>> GetByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(d => d.AssignedToUserId == userId, cancellationToken);
    }
    
    public async Task<IEnumerable<Deal>> GetByStageAsync(DealStage stage, CancellationToken cancellationToken = default)
    {
        return await FindAsync(d => d.Stage == stage, cancellationToken);
    }
}


