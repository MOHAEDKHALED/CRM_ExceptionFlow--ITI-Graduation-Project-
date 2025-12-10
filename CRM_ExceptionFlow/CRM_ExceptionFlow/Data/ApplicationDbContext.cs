using CRM_ExceptionFlow.Models;
using Microsoft.EntityFrameworkCore;
using Exception = CRM_ExceptionFlow.Models.Exception;

namespace CRM_ExceptionFlow.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Deal> Deals { get; set; }
        public DbSet<Interaction> Interactions { get; set; }
        public DbSet<Exception> Exceptions { get; set; }
        public DbSet<AIRecommendation> AIRecommendations { get; set; }
        public DbSet<ExceptionHistory> ExceptionHistory { get; set; }
        public DbSet<Lead> Leads { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email).IsUnique();

            // Cascade Paths

            // Deals -> Users (Restrict)
            modelBuilder.Entity<Deal>()
                .HasOne(d => d.AssignedToUser)
                .WithMany(u => u.Deals)
                .HasForeignKey(d => d.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict); 

            // Deals -> Customers (Cascade)
            modelBuilder.Entity<Deal>()
                .HasOne(d => d.Customer)
                .WithMany(c => c.Deals)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Interactions -> Users (Restrict)
            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.User)
                .WithMany(u => u.Interactions)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);  

            // Interactions -> Customers (Cascade)
            modelBuilder.Entity<Interaction>()
                .HasOne(i => i.Customer)
                .WithMany(c => c.Interactions)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customers -> Users (Restrict)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.AssignedToUser)
                .WithMany(u => u.Customers)
                .HasForeignKey(c => c.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);  

            // Exception -> Users (Already Restrict)
            modelBuilder.Entity<Exception>()
                .HasOne(e => e.ReportedByUser)
                .WithMany(u => u.ReportedExceptions)
                .HasForeignKey(e => e.ReportedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exception>()
                .HasOne(e => e.AssignedToUser)
                .WithMany(u => u.AssignedExceptions)
                .HasForeignKey(e => e.AssignedToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Decimal precision
            modelBuilder.Entity<Deal>()
                .Property(d => d.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<AIRecommendation>()
                .Property(a => a.ConfidenceScore)
                .HasPrecision(3, 2);
        }
    }
}