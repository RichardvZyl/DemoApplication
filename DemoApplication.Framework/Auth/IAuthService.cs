using Abstractions.Results;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface IAuthService
{
    Task<IResult<Auth>> AddAsync(AuthModel model);

    Task<IResult> DeleteAsync(Guid id, Guid activeUserId);

    Task<IResult<Guid>> ChangeRole(RoleChangeModel model, Guid activeUserId);
    Task<IResult<string>> ChangePassword(AuthModel model, Guid activeUserId);
    Task<IResult<string>> ChangeLogin(AuthModel model, Guid activeUserId, string newEncryptedEmail);

    Task<IResult<TokenModel>> SignInAsync(SignInModel model);
}
