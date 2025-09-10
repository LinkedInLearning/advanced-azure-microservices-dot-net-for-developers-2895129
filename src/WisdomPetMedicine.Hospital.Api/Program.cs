using Asp.Versioning;
using Azure.Monitor.OpenTelemetry.Exporter;
using Elastic.Serilog.Sinks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using WisdomPetMedicine.Hospital.Api.ApplicationServices;
using WisdomPetMedicine.Hospital.Api.Infrastructure;
using WisdomPetMedicine.Hospital.Api.IntegrationEvents;
using WisdomPetMedicine.Hospital.Domain.Repositories;
using WisdomPetMedicine.Hospital.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.MinimumLevel.Information();
    config.WriteTo.ApplicationInsights(context.Configuration["AppInsights:ConnectionString"], TelemetryConverter.Events);
    config.WriteTo.Elasticsearch([new Uri("http://localhost:9200")]);
});


builder.Services.AddHealthChecks()
                .AddCosmosDbCheck(builder.Configuration)
                .AddDbContextCheck<HospitalDbContext>();
builder.Services.AddHospitalDb(builder.Configuration);
builder.Services.AddSingleton<IPatientAggregateStore, PatientAggregateStore>();
builder.Services.AddScoped<HospitalApplicationService>();
builder.Services.AddHostedService<PetTransferredToHospitalIntegrationEventHandler>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = Literals.ServiceName, Version = "v1" });
});
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new QueryStringApiVersionReader("api-version");

    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
}).AddApiExplorer(setup =>
{
    setup.GroupNameFormat = "'v'VVV";
    setup.SubstituteApiVersionInUrl = true;
});
builder.Services.AddOpenTelemetry()
    .ConfigureResource(res =>
    {
        res.AddService(Literals.ServiceName);
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAzureMonitorTraceExporter(c =>
            {
                c.ConnectionString = builder.Configuration["AppInsights:ConnectionString"];
            })
            .AddSource("hospital-api")
            .AddOtlpExporter()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation(s => s.SetDbStatementForText = true);
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", Literals.ServiceName));
}
app.EnsureHospitalDbIsCreated();
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();

internal static class Literals
{
    public const string ServiceName = "WisdomPetMedicine.Hospital.Api";
}