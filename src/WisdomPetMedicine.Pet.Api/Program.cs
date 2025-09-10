using Azure.Monitor.OpenTelemetry.Exporter;
using Elastic.Serilog.Sinks;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using WisdomPetMedicine.Pet.Api.ApplicationServices;
using WisdomPetMedicine.Pet.Api.Infrastructure;
using WisdomPetMedicine.Pet.Domain.Repositories;
using WisdomPetMedicine.Pet.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
{
    config.MinimumLevel.Information();
    config.WriteTo.ApplicationInsights(context.Configuration["AppInsights:ConnectionString"], TelemetryConverter.Events);
    config.WriteTo.Elasticsearch([new Uri("http://localhost:9200")]);
});

builder.Services.AddOpenApi();
builder.Services.AddHealthChecks()
                .AddDbContextCheck<PetDbContext>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = Literals.ServiceName, Version = "v1" });
});
builder.Services.AddPetDb(builder.Configuration);
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<PetApplicationService>();
builder.Services.AddScoped<IBreedService, FakeBreedService>();
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
            .AddSource("pet-api")
            .AddOtlpExporter()
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddSqlClientInstrumentation(s => s.SetDbStatementForText = true);
    });
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); 
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", Literals.ServiceName));
}

app.EnsurePetDbIsCreated();
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");
app.Run();

internal class Literals
{
    public static string ServiceName = "WisdomPetMedicine.Pet.Api";
}