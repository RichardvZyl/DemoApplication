using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApplication.UniqueCodeGenerator;
public interface IUniqueCodeSeed
{
    Task<bool> Start(IServiceCollection serviceCollection);
}
