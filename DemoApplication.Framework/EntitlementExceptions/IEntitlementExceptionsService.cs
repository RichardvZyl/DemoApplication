using System;
using System.Threading.Tasks;
using Abstractions.Results;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface IEntitlementExceptionsService
{
    Task<IResult> AddAsync(EntitlementExceptionsModel model, string email, Guid activeUserId);

    Task<IResult<string>> GetRoleByUserEmailAsync(string userEmail);
    Task<EntitlementExceptionsModel> GetByUserIdAsync(Guid id);
    Task<EntitlementExceptionsModel> GetByUserEmailAsync(string email);

    Task<IResult> DeleteAsync(string email, Guid activeUserId);

    bool ModelHasExceptions(EntitlementExceptionsModel model);
}
