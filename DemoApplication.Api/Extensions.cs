using System.Linq;
using Abstractions.AspNetCore;
using Abstractions.IoC;
using DemoApplication.Database;
using DemoApplication.Framework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace DemoApplication.Api;

/// <summary> Add versioning functionality </summary>
public class RemoveVersionParameterFilter : IOperationFilter
{
    /// <summary> Add versioning functionality to Swagger </summary>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var versionParameter = operation.Parameters.Single(p => p.Name == "version");
        _ = operation.Parameters.Remove(versionParameter);
    }
}

/// <summary> Add current version </summary>
public class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
{
    /// <summary> Add current version </summary>
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var paths = new OpenApiPaths();
        foreach (var path in swaggerDoc.Paths)
        {
            paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
        }
        swaggerDoc.Paths = paths;
    }
}

/// <summary> Add functionality to existing features </summary>
public static class Extensions
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
