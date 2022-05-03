using Abstractions.EntityFrameworkCore;
using DemoApplication.Domain;
using DemoApplication.Enums;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database;

public sealed class AuthRepository : EFRepository<Auth>, IAuthRepository
{
    public AuthRepository(Context context) : base(context) { }

    public async Task<bool> AnyByLoginAsync(string login)
        => await Queryable.AnyAsync(AuthExpression.Login(login)!);

    public async Task<Auth> GetByLoginAsync(string login)
        => await Queryable.SingleOrDefaultAsync(AuthExpression.Login(login)!) ?? new(Guid.Empty);

    public async Task<RolesEnum> GetRoleByEmailAsync(string login)
        => await Queryable.Where(AuthExpression.Login(login)!).Select(AuthExpression.Role()!).SingleOrDefaultAsync();


    public Task UpdateRoleAsync(Auth auth) => UpdatePartialAsync(auth.Id, new { auth.Role });
    public Task UpdateLogin(Auth auth) => UpdatePartialAsync(auth.Id, new { auth.Email });
    public Task UpdatePassword(Auth auth) => UpdatePartialAsync(auth.Id, new { auth.Password });
}
