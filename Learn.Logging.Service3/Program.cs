using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;

namespace Learn.Logging.Service3
{
    public class Program
    {
        public static int Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;

            Log.Logger = new LoggerConfiguration()
              .MinimumLevel.Information()
              .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
              .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
              .Enrich.WithProperty("Replica", 5)
              .Enrich.WithProperty("App", nameof(Service3))
              .Enrich.FromLogContext()
              .WriteTo.Console()

              .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
              {
                  AutoRegisterTemplate = true,
                  AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
              })

              .CreateLogger();

            try
            {
                Log.Information("Starting web host");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
              .CreateDefaultBuilder(args)
              .UseSerilog() // <-- Add this line
              .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}