using Microsoft.EntityFrameworkCore;
using CRM.Domain.Entities;

namespace CRM.Infrastructure.Data;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Customer> Customers { get; }
    DbSet<CRM.Domain.Entities.Exception> Exceptions { get; }
    DbSet<Deal> Deals { get; }
    DbSet<Interaction> Interactions { get; }
    DbSet<AIRecommendation> AIRecommendations { get; }
    DbSet<ExceptionHistory> ExceptionHistory { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}


