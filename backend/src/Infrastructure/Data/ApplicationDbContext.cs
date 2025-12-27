using Microsoft.EntityFrameworkCore;
using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;
using System.Reflection;
using DomainException = CRM.Domain.Entities.Exception;

namespace CRM.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<DomainException> Exceptions => Set<DomainException>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<Interaction> Interactions => Set<Interaction>();
    public DbSet<AIRecommendation> AIRecommendations => Set<AIRecommendation>();
    public DbSet<ExceptionHistory> ExceptionHistory => Set<ExceptionHistory>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.HasIndex(u => u.Email).IsUnique();
            
            entity.Property(u => u.Username).HasMaxLength(50);
            entity.Property(u => u.Email).HasMaxLength(100);
            entity.Property(u => u.PasswordHash).HasMaxLength(256);
            entity.Property(u => u.FullName).HasMaxLength(100);
            entity.Property(u => u.Department).HasMaxLength(50);
            
            // Configure UserRole as Value Object (store as string)
            entity.Property(u => u.Role)
                .HasConversion(
                    v => v.Value,
                    v => UserRole.FromString(v))
                .HasMaxLength(50);
            
            entity.Ignore(u => u.DomainEvents);
        });
        
        // Ignore Value Objects (they are not entities)
        modelBuilder.Ignore<UserRole>();
        
        // Customer configuration
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(c => c.Name).HasMaxLength(100);
            entity.Property(c => c.Email).HasMaxLength(100);
            entity.Property(c => c.Phone).HasMaxLength(20);
            entity.Property(c => c.Company).HasMaxLength(100);
            entity.Property(c => c.Address).HasMaxLength(200);
            
            entity.HasOne(c => c.AssignedToUser)
                .WithMany(u => u.Customers)
                .HasForeignKey(c => c.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Ignore(c => c.DomainEvents);
        });
        
        // Exception configuration
        modelBuilder.Entity<DomainException>(entity =>
        {
            entity.Property(e => e.ProjectId).HasMaxLength(50);
            entity.Property(e => e.Module).HasMaxLength(50);
            entity.Property(e => e.Title).HasMaxLength(200);
            
            entity.HasOne(e => e.ReportedByUser)
                .WithMany(u => u.ReportedExceptions)
                .HasForeignKey(e => e.ReportedByUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.AssignedToUser)
                .WithMany(u => u.AssignedExceptions)
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Ignore(e => e.DomainEvents);
        });
        
        // Deal configuration
        modelBuilder.Entity<Deal>(entity =>
        {
            entity.Property(d => d.Title).HasMaxLength(200);
            entity.Property(d => d.Amount).HasPrecision(18, 2);
            
            entity.HasOne(d => d.Customer)
                .WithMany(c => c.Deals)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(d => d.AssignedToUser)
                .WithMany(u => u.Deals)
                .HasForeignKey(d => d.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.Ignore(d => d.DomainEvents);
        });
        
        // Interaction configuration
        modelBuilder.Entity<Interaction>(entity =>
        {
            entity.Property(i => i.Subject).HasMaxLength(200);
            
            entity.HasOne(i => i.Customer)
                .WithMany(c => c.Interactions)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasOne(i => i.User)
                .WithMany(u => u.Interactions)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        // AIRecommendation configuration
        modelBuilder.Entity<AIRecommendation>(entity =>
        {
            entity.Property(a => a.Model).HasMaxLength(50);
            entity.Property(a => a.ConfidenceScore).HasPrecision(3, 2);
            
            entity.HasOne(a => a.Exception)
                .WithMany(e => e.AIRecommendations)
                .HasForeignKey(a => a.ExceptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        // ExceptionHistory configuration
        modelBuilder.Entity<ExceptionHistory>(entity =>
        {
            entity.Property(e => e.ChangedByUserName).HasMaxLength(100);
            
            entity.HasOne(e => e.Exception)
                .WithMany(ex => ex.History)
                .HasForeignKey(e => e.ExceptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

