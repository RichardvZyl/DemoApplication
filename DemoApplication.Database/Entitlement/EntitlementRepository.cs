using System;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.EntityFrameworkCore;
using DemoApplication.Domain;
using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database;

public sealed class EntitlementRepository : EFRepository<Entitlement>, IEntitlementRepository
{
    public EntitlementRepository(Context context) : base(context) { }


    public async Task<EntitlementModel> GetModelByUserIdAsync(Guid id)
        => await Queryable.Where(EntitlementExpression.UserId(id)!).Select(EntitlementExpression.Model!).SingleOrDefaultAsync() ?? new();

    public async Task<bool> AnyByUserIdAsync(Guid userId)
        => await Queryable.AnyAsync(EntitlementExpression.UserId(userId)!);

    public async Task UpdateAsync(Entitlement exceptions)
        => await UpdatePartialAsync(exceptions.UserId, new { exceptions.AuditLogs, exceptions.AuditReport, exceptions.AuthorizeMakerChecker, exceptions.EntitlementChange, exceptions.StatisticalReport, exceptions.SuspendUsers, exceptions.ViewNotifications, exceptions.ViewUsers });
}
