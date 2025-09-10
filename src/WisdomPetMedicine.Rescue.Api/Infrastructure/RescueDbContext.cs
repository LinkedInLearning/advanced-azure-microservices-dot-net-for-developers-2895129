﻿using Microsoft.EntityFrameworkCore;
using WisdomPetMedicine.Rescue.Api.IntegrationEvents;
using WisdomPetMedicine.Rescue.Domain.Entities;

namespace WisdomPetMedicine.Rescue.Api.Infrastructure;

public class RescueDbContext(DbContextOptions<RescueDbContext> options) : DbContext(options)
{
    public DbSet<Adopter> Adopters { get; set; }
    public DbSet<RescuedAnimal> RescuedAnimals { get; set; }
    public DbSet<PetFlaggedForAdoptionIntegrationEvent> RescuedAnimalsMetadata { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Adopter>().HasKey(x => x.Id);
        modelBuilder.Entity<Adopter>().OwnsOne(x => x.Name);
        modelBuilder.Entity<Adopter>().OwnsOne(x => x.Questionnaire);
        modelBuilder.Entity<Adopter>().OwnsOne(x => x.Address);
        modelBuilder.Entity<Adopter>().OwnsOne(x => x.PhoneNumber);
        modelBuilder.Entity<RescuedAnimal>().HasKey(x => x.Id);
        modelBuilder.Entity<RescuedAnimal>().OwnsOne(x => x.AdopterId);
    }
}

public static class RescuedAnimalDbContextExtensions
{
    public static void AddRescueDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RescueDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Rescue"));
        });
    }
    public static void EnsureRescueDbIsCreated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetService<RescueDbContext>();
        context.Database.EnsureCreated();
        context.Database.CloseConnection();
    }
}