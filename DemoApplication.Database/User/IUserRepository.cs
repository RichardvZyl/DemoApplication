using Abstractions.Repositories;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database;

public interface IUserRepository : IRepository<User>
{
    Task<Guid> GetAuthIdByUserIdAsync(Guid id);

    Task<string> GetFullNameByUserId(Guid id);

    Task<string> GetEmailById(Guid id);

    Task<bool> GetIsActive(Guid id);

    Task<UserModel> GetByIdAsync(Guid id);

    Task<UserModel> GetByAuthIdAsync(Guid id);

    Task<Guid> GetIdByEmail(string email);

    Task<bool> AnyByEmailAsync(string email);

    Task UpdateStatusAsync(User user);
}
