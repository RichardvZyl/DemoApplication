using System;
using System.Threading.Tasks;
using Abstractions.Repositories;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database;

public interface IEntitlementExceptionsRepository : IRepository<EntitlementExceptions>
{
    Task<bool> AnyByUserIdAsync(Guid userId);
    Task<EntitlementExceptionsModel> GetModelByUserIdAsync(Guid id);
    Task UpdateAsync(EntitlementExceptions exceptions);
}
