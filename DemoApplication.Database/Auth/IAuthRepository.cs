using System.Threading.Tasks;
using Abstractions.Repositories;
using DemoApplication.Domain;
using DemoApplication.Enums;

namespace DemoApplication.Database;

public interface IAuthRepository : IRepository<Auth>
{
    Task<bool> AnyByLoginAsync(string login);

    Task<RolesEnum> GetRoleByEmailAsync(string login);

    Task<Auth> GetByLoginAsync(string login);
    Task UpdateRoleAsync(Auth auth);

    Task UpdateLogin(Auth auth);
    Task UpdatePassword(Auth auth);
}
