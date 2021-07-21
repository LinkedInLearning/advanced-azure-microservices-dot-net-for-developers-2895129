using Azure.Monitor.OpenTelemetry.Exporter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using WisdomPetMedicine.Pet.Api.ApplicationServices;
using WisdomPetMedicine.Pet.Api.Infrastructure;
using WisdomPetMedicine.Pet.Domain.Repositories;
using WisdomPetMedicine.Pet.Domain.Services;

namespace WisdomPetMedicine.Pet.Api
{
    public class Startup
    {
        private const string ServiceName = "WisdomPetMedicine.Pet.Api";
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
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
                }).AddSource("pet-api")
                  .AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddSqlClientInstrumentation(s => s.SetDbStatementForText = true);
            });
            services.AddHealthChecks()
                    .AddDbContextCheck<PetDbContext>();
            services.AddPetDb(Configuration);
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<PetApplicationService>();
            services.AddScoped<IBreedService, FakeBreedService>();
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
            app.EnsurePetDbIsCreated();
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