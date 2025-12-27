using CRM.Domain.Entities;
using CRM.Domain.Repositories;
using CRM.Infrastructure.Data;
using DomainException = CRM.Domain.Entities.Exception;

namespace CRM.Infrastructure.Repositories;

public class ExceptionRepository : BaseRepository<DomainException, int>, IExceptionRepository
{
    public ExceptionRepository(ApplicationDbContext context) : base(context)
    {
    }
    
    public async Task<IEnumerable<DomainException>> GetByStatusAsync(ExceptionStatus status, CancellationToken cancellationToken = default)
    {
        return await FindAsync(e => e.Status == status, cancellationToken);
    }
    
    public async Task<IEnumerable<DomainException>> GetByPriorityAsync(ExceptionPriority priority, CancellationToken cancellationToken = default)
    {
        return await FindAsync(e => e.Priority == priority, cancellationToken);
    }
    
    public async Task<IEnumerable<DomainException>> GetByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(e => e.AssignedToUserId == userId, cancellationToken);
    }
    
    public async Task<IEnumerable<DomainException>> GetByReportedUserIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return await FindAsync(e => e.ReportedByUserId == userId, cancellationToken);
    }
}

