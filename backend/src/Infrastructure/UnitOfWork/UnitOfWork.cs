using CRM.Domain.Repositories;
using CRM.Domain.UnitOfWork;
using CRM.Infrastructure.Data;
using CRM.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace CRM.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction? _transaction;
    
    private IUserRepository? _users;
    private ICustomerRepository? _customers;
    private IExceptionRepository? _exceptions;
    private IDealRepository? _deals;
    private IInteractionRepository? _interactions;
    
    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public IUserRepository Users => _users ??= new UserRepository(_context);
    public ICustomerRepository Customers => _customers ??= new CustomerRepository(_context);
    public IExceptionRepository Exceptions => _exceptions ??= new ExceptionRepository(_context);
    public IDealRepository Deals => _deals ??= new DealRepository(_context);
    public IInteractionRepository Interactions => _interactions ??= new InteractionRepository(_context);
    
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.CommitAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync(cancellationToken);
            await _transaction.DisposeAsync();
            _transaction = null;
        }
    }
    
    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}


