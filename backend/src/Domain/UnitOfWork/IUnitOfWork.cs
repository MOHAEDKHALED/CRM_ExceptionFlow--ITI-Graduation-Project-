using CRM.Domain.Repositories;

namespace CRM.Domain.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    ICustomerRepository Customers { get; }
    IExceptionRepository Exceptions { get; }
    IDealRepository Deals { get; }
    IInteractionRepository Interactions { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}


