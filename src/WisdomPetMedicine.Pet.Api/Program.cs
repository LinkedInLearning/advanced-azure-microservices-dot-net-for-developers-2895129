using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace WisdomPetMedicine.Pet.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(LogLevel.Debug);
                })
                .UseSerilog((context, config) =>
                {
                    config.MinimumLevel.Information();
                    config.WriteTo.ApplicationInsights(context.Configuration["AppInsights:InstrumentationKey"], TelemetryConverter.Events);
                    var elasticSearchOptions = new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri(context.Configuration["Elastic:Url"]))
                    {
                        AutoRegisterTemplate = true,
                        AutoRegisterTemplateVersion = Serilog.Sinks.Elasticsearch.AutoRegisterTemplateVersion.ESv7,
                        IndexFormat = "wisdompetmedicine-{0:yyyy.MM.dd}",
                        MinimumLogEventLevel = Serilog.Events.LogEventLevel.Debug
                    };
                    config.WriteTo.Elasticsearch(elasticSearchOptions);
                });
    }
}