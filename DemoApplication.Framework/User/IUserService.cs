using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Results;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface IUserService
{
    Task<IResult<Guid>> AddAsync(UserModel model, Guid activeUserId);

    Task<UserModel> GetAsync(Guid id, RolesEnum role);
    Task<IEnumerable<UserModel>> ListAsync(RolesEnum role);

    Task<IResult> SuspendAsync(Guid id, Guid activeUserId);
    Task<IResult> UpdateAsync(UserModel model);

    Task<IResult> DeleteAsync(Guid id, Guid activeUserId);

}
