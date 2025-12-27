using CRM.Domain.Entities;

namespace CRM.Domain.Repositories;

public interface ICustomerRepository : IRepository<Customer, int>
{
    Task<IEnumerable<Customer>> GetByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default);
}


