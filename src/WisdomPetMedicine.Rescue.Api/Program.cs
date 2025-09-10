using Microsoft.OpenApi.Models;
using WisdomPetMedicine.Rescue.Api.ApplicationServices;
using WisdomPetMedicine.Rescue.Api.Infrastructure;
using WisdomPetMedicine.Rescue.Api.IntegrationEvents;
using WisdomPetMedicine.Rescue.Domain.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRescueDb(builder.Configuration);
builder.Services.AddScoped<AdopterApplicationService>();
builder.Services.AddScoped<IRescueRepository, RescueRepository>();
builder.Services.AddHostedService<PetFlaggedForAdoptionIntegrationEventHandler>();
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WisdomPetMedicine.Rescue.Api", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WisdomPetMedicine.Rescue.Api v1"));
}

app.EnsureRescueDbIsCreated();
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();