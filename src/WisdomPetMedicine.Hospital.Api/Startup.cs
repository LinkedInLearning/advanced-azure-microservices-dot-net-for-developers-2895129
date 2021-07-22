using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WisdomPetMedicine.Hospital.Api.ApplicationServices;
using WisdomPetMedicine.Hospital.Api.Infrastructure;
using WisdomPetMedicine.Hospital.Api.IntegrationEvents;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Infrastructure;

namespace WisdomPetMedicine.Hospital.Api
{
    public class Startup
    {
        private const string ServiceName = "WisdomPetMedicine.Hospital.Api";

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            services.AddOpenTelemetryTracing(config =>
            {
                config.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(ServiceName));
                config.AddAzureMonitorTraceExporter(c =>
                {
                    c.ConnectionString = Configuration["AppInsights:ConnectionString"];
                })
                .AddJaegerExporter(o =>
                {
                    o.AgentHost = Configuration.GetValue<string>("Jaeger:Host");
                    o.AgentPort = Configuration.GetValue<int>("Jaeger:Port");
                }).AddSource("hospital-api")
                  .AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddSqlClientInstrumentation(s => s.SetDbStatementForText = true);
            });
            services.AddHealthChecks()
                    .AddCosmosDbCheck(Configuration)
                    .AddDbContextCheck<HospitalDbContext>();
            services.AddHospitalDb(Configuration);
            services.AddSingleton<IPatientAggregateStore, PatientAggregateStore>();
            services.AddScoped<HospitalApplicationService>();
            services.AddHostedService<PetTransferredToHospitalIntegrationEventHandler>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = ServiceName, Version = "v1" });
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", ServiceName));
            }
            app.EnsureHospitalDbIsCreated();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllers();
            });
        }
    }
}