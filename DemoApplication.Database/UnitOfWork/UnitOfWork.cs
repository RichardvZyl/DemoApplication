using System.Threading.Tasks;

namespace DemoApplication.Database;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly Context _context;

    public UnitOfWork(Context context) 
        => _context = context;

    //TODO: Implement returning an IResult.Failed on exception
    //TODO: Implement Audit Trail for all Saved changes
    public async Task<int> SaveChangesAsync() 
        => await _context.SaveChangesAsync();
}
