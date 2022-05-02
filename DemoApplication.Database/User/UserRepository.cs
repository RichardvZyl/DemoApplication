using System;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.EntityFrameworkCore;
using DemoApplication.Domain;
using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database;

public sealed class UserRepository : EFRepository<User>, IUserRepository
{
    public UserRepository(Context context) : base(context) { }

    public async Task<Guid> GetAuthIdByUserIdAsync(Guid id) => await Queryable.Where(UserExpression.Id(id)!).Select(UserExpression.AuthId()!).SingleOrDefaultAsync();

    public async Task<UserModel> GetByAuthIdAsync(Guid id) => await Queryable.Where(UserExpression.AuthId(id)!).Select(UserExpression.Model!).SingleOrDefaultAsync() ?? new();

    public async Task<UserModel> GetByIdAsync(Guid id) => await Queryable.Where(UserExpression.Id(id)!).Select(UserExpression.Model!).SingleOrDefaultAsync() ?? new();

    public async Task<string> GetEmailById(Guid id) => await Queryable.Where(UserExpression.Id(id)!).Select(UserExpression.Email()!).SingleOrDefaultAsync() ?? string.Empty;

    public async Task<string> GetFullNameByUserId(Guid id) => await Queryable.Where(UserExpression.Id(id)!).Select(UserExpression.FullName()!).SingleOrDefaultAsync() ?? string.Empty;

    public async Task<Guid> GetIdByEmail(string email) => await Queryable.Where(UserExpression.Email(email)!).Select(UserExpression.Id()!).SingleOrDefaultAsync();

    public async Task<bool> GetIsActive(Guid id) => await Queryable.Where(UserExpression.Id(id)!).Select(UserExpression.Active()!).SingleOrDefaultAsync();

    public async Task<bool> AnyByEmailAsync(string email) => await Queryable.AnyAsync(UserExpression.Email(email)!);

    public Task UpdateStatusAsync(User user) => UpdatePartialAsync(user.Id, new { user.Active });
}
