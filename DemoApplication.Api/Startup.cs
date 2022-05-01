using System;
using System.IO;
using System.Reflection;
using Abstractions.AspNetCore;
using Abstractions.EntityFrameworkCore;
using Abstractions.Security;
using Abstractions.SwaggerExtension;
using DemoApplication.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApplication.Api;

/// <summary>  Used for configuration on application start </summary>
public class Startup
{
    //private readonly IConfiguration _configuration;

    //public Startup(IConfiguration configuration)
    //{
    //    _configuration = configuration;
    //}

    /// <summary> Configure application functionality </summary>
    public void Configure(IApplicationBuilder application)
    {
        application.UseException();
        application.UseCorsAllowAny();
        application.UseHttps();
        application.UseRouting();

        application.UseStaticFiles();
        //application.UseStaticFiles(new StaticFileOptions()
        //{
        //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory()), @"Resources")),
        //    RequestPath = new PathString("/Resources")
        //});

        application.UseResponseCompression();
        application.UseResponseCaching();
        application.UseAuthentication();
        application.UseAuthorization();

        application.AddSwagger();

        application.UseEndpoints();
    }

    /// <summary>  Configure application Services </summary>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddSecurity();
        services.AddCryptography(Abstractions.IoC.IoCExtensions.GetSecrets(services, "CryptoKey"));
        services.AddResponseCompression();
        services.AddResponseCaching();
        services.AddControllersDefault();

        //services.AddContextUsePostgreSQL<Context>();
        //services.AddContextUseSQL<Context>();
        services.AddContextUseMemory<Context>();

        services.AddApiVersioning();

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        services.AddSwagger(xmlPath);

        services.AddControllersWithViews();

        services.AddServices();
    }
}
