using CRM.Domain.Entities;

namespace CRM.Domain.Repositories;

public interface IExceptionRepository : IRepository<CRM.Domain.Entities.Exception, int>
{
    Task<IEnumerable<CRM.Domain.Entities.Exception>> GetByStatusAsync(ExceptionStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<CRM.Domain.Entities.Exception>> GetByPriorityAsync(ExceptionPriority priority, CancellationToken cancellationToken = default);
    Task<IEnumerable<CRM.Domain.Entities.Exception>> GetByAssignedUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<CRM.Domain.Entities.Exception>> GetByReportedUserIdAsync(int userId, CancellationToken cancellationToken = default);
}

