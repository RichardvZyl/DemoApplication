using System;
using System.Threading.Tasks;
using Abstractions.Repositories;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database;

public interface IEntitlementRepository : IRepository<Entitlement>
{
    Task<bool> AnyByUserIdAsync(Guid userId);
    Task<EntitlementModel> GetModelByUserIdAsync(Guid id);
    Task UpdateAsync(Entitlement exceptions);
}
