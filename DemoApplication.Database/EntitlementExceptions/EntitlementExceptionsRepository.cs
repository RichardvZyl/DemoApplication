using Abstractions.EntityFrameworkCore;
using DemoApplication.Domain;
using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database;

public sealed class EntitlementExceptionsRepository : EFRepository<EntitlementExceptions>, IEntitlementExceptionsRepository
{
    public EntitlementExceptionsRepository(Context context) : base(context) { }


    public async Task<EntitlementExceptionsModel> GetModelByUserIdAsync(Guid id)
        => await Queryable.Where(EntitlementExceptionsExpression.UserId(id)!).Select(EntitlementExceptionsExpression.Model!).SingleOrDefaultAsync() ?? new();

    public async Task<bool> AnyByUserIdAsync(Guid userId)
        => await Queryable.AnyAsync(EntitlementExceptionsExpression.UserId(userId)!);

    public async Task UpdateAsync(EntitlementExceptions exceptions)
        => await UpdatePartialAsync(exceptions.UserId, new { exceptions.AuditLogs, exceptions.AuditReport, exceptions.AuthorizeMakerChecker, exceptions.EntitlementChange, exceptions.ExpiresOn, exceptions.StatisticalReport, exceptions.SuspendUsers, exceptions.ViewNotifications, exceptions.ViewUsers });
}
