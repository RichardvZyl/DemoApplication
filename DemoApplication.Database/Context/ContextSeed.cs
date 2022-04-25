using System;
using Abstractions.Security;
using DemoApplication.Domain;
using DemoApplication.Enums;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database;

public static class ContextSeed
{
    #region Constants
    private static readonly Guid userId = Guid.NewGuid();
    private static readonly Guid clerkId = Guid.NewGuid();
    private static readonly Guid adminSalt = Guid.NewGuid();
    private static readonly Guid clerkSalt = Guid.NewGuid();
    #endregion

    public static void Seed(this ModelBuilder builder)
    {
        //TODO: find a way to inject crypto and hash service OR Encrypt and Hash beforehand

        builder.SeedAuths();
        builder.SeedUsers();
        builder.SeedEntitlement();
        builder.SeedEntitlementExceptions();
    }

    #region Seed Test Values
    private static void SeedAuths(this ModelBuilder builder)
    {
        builder.Entity<Auth>(auth =>
        {
            auth.HasData(new
            {
                Id = userId,
                Login = new CryptographyService("DemoApplicationApplicationUniquePassword").Encrypt("admin@admin.com").GetAwaiter().GetResult(),
                Password = new HashService(10000, 128).Create("admin", adminSalt.ToString()),
                Salt = adminSalt.ToString(),
                Role = RolesEnum.Administrator,
                Email = new CryptographyService("DemoApplicationApplicationUniquePassword").Encrypt("admin@admin.com").GetAwaiter().GetResult(),
                IsDeleted = false,
                LastUpdated = DateTimeOffset.UtcNow
            });

            auth.HasData(new
            {
                Id = clerkId,
                Login = new CryptographyService("DemoApplicationApplicationUniquePassword").Encrypt("clerk@clerk.com").GetAwaiter().GetResult(),
                Password = new HashService(10000, 128).Create("clerk", clerkSalt.ToString()),
                Salt = clerkSalt.ToString(),
                Role = RolesEnum.Clerk,
                Email = new CryptographyService("DemoApplicationApplicationUniquePassword").Encrypt("clerk@clerk.com").GetAwaiter().GetResult(),
                IsDeleted = false,
                LastUpdated = DateTimeOffset.UtcNow
            });
        });
    }

    private static void SeedUsers(this ModelBuilder builder)
    {
        builder.Entity<User>(user 
            =>
            {
                user.HasData(new 
                { 
                    Id = userId,
                    Active = true,
                    AuthId = userId,
                    Email = new CryptographyService("DemoApplicationApplicationUniquePassword").Encrypt("admin@admin.com").GetAwaiter().GetResult(),
                    IsDeleted = false,
                    LastUpdated = DateTimeOffset.UtcNow
                });

                user.OwnsOne(owned => owned.FullName).HasData(new { UserId = userId, Name = "Administrator", Surname = "Administrator" });
            });

        builder.Entity<User>(user 
            =>
            {
                user.HasData(new
                {
                    Id = clerkId,
                    Active = true,
                    AuthId = clerkId,
                    Email = new CryptographyService("DemoApplicationApplicationUniquePassword").Encrypt("clerk@clerk.com").GetAwaiter().GetResult(),
                    IsDeleted = false,
                    LastUpdated = DateTimeOffset.UtcNow
                });

                user.OwnsOne(owned => owned.FullName).HasData(new { UserId = clerkId, Name = "Clerk", Surname = "Clerk" });
            });
    }

    private static void SeedEntitlement(this ModelBuilder builder)
    {
        //TODO: Use Seed entitlement From User Role to create seed for entitlement
    }

    private static void SeedEntitlementExceptions(this ModelBuilder builder) => builder.Entity<EntitlementExceptions>(credential 
        =>
        {
            credential.HasData(new
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ExpiresOn = new DateTimeOffset?(),
                ViewUsers = true,
                AuthorizeMakerChecker = true,
                EntitlementChange = true,
                AuditLogs = true,
                IsDeleted = false,
                LastUpdated = DateTimeOffset.UtcNow
            });
        });
    #endregion
}
