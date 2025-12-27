using System.Linq.Expressions;
using CRM.Domain.Common;
using CRM.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using CRM.Infrastructure.Data;

namespace CRM.Infrastructure.Repositories;

public class BaseRepository<TEntity, TId> : IRepository<TEntity, TId> 
    where TEntity : Entity<TId>
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;
    
    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }
    
    public virtual async Task<TEntity?> GetByIdAsync(TId id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id! }, cancellationToken);
    }
    
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.ToListAsync(cancellationToken);
    }
    
    public virtual async Task<IEnumerable<TEntity>> FindAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }
    
    public virtual async Task<TEntity?> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
    }
    
    public virtual async Task<bool> ExistsAsync(
        Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(predicate, cancellationToken);
    }
    
    public virtual async Task<int> CountAsync(
        Expression<Func<TEntity, bool>>? predicate = null, 
        CancellationToken cancellationToken = default)
    {
        if (predicate == null)
            return await _dbSet.CountAsync(cancellationToken);
        
        return await _dbSet.CountAsync(predicate, cancellationToken);
    }
    
    public virtual void Add(TEntity entity)
    {
        _dbSet.Add(entity);
    }
    
    public virtual void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }
    
    public virtual void Remove(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
}


