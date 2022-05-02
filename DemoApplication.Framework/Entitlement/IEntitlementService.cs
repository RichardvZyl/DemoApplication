using Abstractions.Results;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface IEntitlementService
{
    Task<IResult> AddAsync(EntitlementModel model, string email, Guid activeUserId);

    Task<IResult<string>> GetRoleByUserEmailAsync(string userEmail);
    Task<EntitlementModel> GetByUserIdAsync(Guid id);
    Task<EntitlementModel> GetByUserEmailAsync(string email);
    Task<EntitlementModel> GetDefaultEntitlement(RolesEnum role);

    Task<IResult> DeleteAsync(string email, Guid activeUserId);
}
