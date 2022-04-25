using System.Threading.Tasks;

namespace DemoApplication.Database;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync();
}
