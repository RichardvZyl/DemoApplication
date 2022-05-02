using Abstractions.AspNetCore;
using Abstractions.IoC;
using DemoApplication.Database;
using DemoApplication.Framework;

namespace DemoApplication.Api;

/// <summary> Add functionality to existing features </summary>
public static class IoCExtensions
{

    /// <summary> Add Services to be used by application for dependency injection </summary>
    public static void AddServices(this IServiceCollection services)
    {
        services.AddFileExtensionContentTypeProvider();

        services.AddClassesInterfaces(typeof(IUnitOfWork).Assembly);
        services.AddClassesInterfaces(typeof(IAuthService).Assembly);
        services.AddClassesInterfaces(typeof(IUserService).Assembly);
        services.AddClassesInterfaces(typeof(IAuditTrailService).Assembly);
        services.AddClassesInterfaces(typeof(IMakerCheckerService).Assembly);
        services.AddClassesInterfaces(typeof(INotificationService).Assembly);
        services.AddClassesInterfaces(typeof(IEntitlementService).Assembly);
        services.AddClassesInterfaces(typeof(IEntitlementExceptionsService).Assembly);
        //services.AddClassesInterfaces(typeof(IUniqueCodeSeed).Assembly);
    }

}
