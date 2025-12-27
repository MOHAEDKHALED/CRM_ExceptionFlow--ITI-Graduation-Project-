using CRM.Domain.Entities;
using CRM.Domain.Repositories;
using CRM.Infrastructure.Data;

namespace CRM.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer, int>, ICustomerRepository
{
    public CustomerRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<Customer>> GetByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(c => c.AssignedToUserId == userId, cancellationToken);
    }
}


