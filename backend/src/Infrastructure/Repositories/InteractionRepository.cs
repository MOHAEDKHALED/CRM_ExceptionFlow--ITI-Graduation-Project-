using CRM.Domain.Entities;
using CRM.Domain.Repositories;
using CRM.Infrastructure.Data;

namespace CRM.Infrastructure.Repositories;

public class InteractionRepository : BaseRepository<Interaction, int>, IInteractionRepository
{
    public InteractionRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Interaction>> GetByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(i => i.CustomerId == customerId, cancellationToken);
    }
    
    public async Task<IEnumerable<Interaction>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(i => i.UserId == userId, cancellationToken);
    }
}


