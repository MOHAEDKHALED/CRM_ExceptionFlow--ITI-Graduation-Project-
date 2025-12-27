using CRM.Domain.Entities;
using CRM.Domain.ValueObjects;
using CRM.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRM.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        // Check if admin user exists
        if (await context.Users.AnyAsync(u => u.Username == "admin"))
            return; // Admin already exists

        // Create default admin user
        var adminPasswordHash = passwordHasher.HashPassword("Admin@123");
        var adminUser = new User(
            username: "admin",
            email: "admin@crm.com",
            passwordHash: adminPasswordHash,
            fullName: "System Administrator",
            role: UserRole.Admin,
            department: "IT"
        );

        context.Users.Add(adminUser);
        await context.SaveChangesAsync();
    }
}

