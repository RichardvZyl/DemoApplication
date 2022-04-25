using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Abstractions.AspNetCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Abstractions.EntityFrameworkCore;
using DemoApplication.Database;
using Abstractions.Security;
using Abstractions.IoC;

//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Formatters;
//using Microsoft.Extensions.Options;

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

        application.UseSwagger();
        application.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1");
            // Specify an endpoint for v2
            c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"v2");
        });

        application.UseEndpoints();
    }

    /// <summary>  Configure application Services </summary>
    public void ConfigureServices(IServiceCollection services)
    {

        services.AddCors();
        services.AddSecurity();
        services.AddCryptography(IoCExtensions.GetSecrets(services, "CryptoKey"));
        services.AddResponseCompression();
        services.AddResponseCaching();
        services.AddControllersDefault();
        
        //services.AddContextUsePostgreSQL<Context>();
        //services.AddContextUseSQL<Context>();
        services.AddContextUseMemory<Context>();
        
        services.AddApiVersioning();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "DemoApplicationAPI v1", Version = "v1" });
            // Add a SwaggerDoc for v2 
            c.SwaggerDoc("v2",
                new OpenApiInfo
                {
                    Version = "v2",
                    Title = "DemoApplicationAPI v1"
                });
            c.OperationFilter<RemoveVersionParameterFilter>();
            c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            c.EnableAnnotations();

            //c.DocInclusionPredicate((version, desc) =>
            //{
            //var versions = desc.ControllerAttributes()
            //    .OfType<ApiVersionAttribute>()
            //    .SelectMany(attr => attr.Versions);

            //var maps = desc.ActionAttributes()
            //    .OfType<MapToApiVersionAttribute>()
            //    .SelectMany(attr => attr.Versions)
            //    .ToArray();

            //    return versions.Any(v => $"v{v.ToString()}" == version)
            //                  && (!maps.Any() || maps.Any(v => $"v{v.ToString()}" == version)); ;
            //});

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' following by space and JWT",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });

        services
            .AddControllersWithViews()
            .AddNewtonsoftJson();

        services.AddServices();
    }

    /// <summary> JSonPatch Input Formatter </summary>
    ////private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    ////{
    ////    var builder = new ServiceCollection()
    ////        .AddLogging()
    ////        .AddMvc()
    ////        .AddNewtonsoftJson()
    ////        .Services.BuildServiceProvider();

    ////    return builder
    ////        .GetRequiredService<IOptions<MvcOptions>>()
    ////        .Value
    ////        .InputFormatters
    ////        .OfType<NewtonsoftJsonPatchInputFormatter>()
    ////        .First();
    ////}
}
