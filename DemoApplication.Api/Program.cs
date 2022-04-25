using Microsoft.Extensions.Hosting;
using Abstractions.AspNetCore;
using Abstractions.Logging;

namespace DemoApplication.Api;

/// <summary> The main class of the program </summary>
public static class Program
{
    /// <summary> The starting point of the application </summary>
    public static void Main() 
        => Host.CreateDefaultBuilder().UseSerilog().Run<Startup>();
        //HttpConfiguration config = GlobalConfiguration.Configuration;
        //config.Formatters.JsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fff+00:00";

}
