using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WisdomPetMedicine.Pet.Api.Infrastructure
{
    public class PetDbContext : DbContext
    {
        public DbSet<Domain.Entities.Pet> Pets { get; set; }

        public PetDbContext(DbContextOptions<PetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Domain.Entities.Pet>().HasKey(x => x.Id);
            modelBuilder.Entity<Domain.Entities.Pet>().OwnsOne(x => x.Name);
            modelBuilder.Entity<Domain.Entities.Pet>().OwnsOne(x => x.Breed);
            modelBuilder.Entity<Domain.Entities.Pet>().OwnsOne(x => x.SexOfPet);
            modelBuilder.Entity<Domain.Entities.Pet>().OwnsOne(x => x.Species);
            modelBuilder.Entity<Domain.Entities.Pet>().OwnsOne(x => x.Color);
            modelBuilder.Entity<Domain.Entities.Pet>().OwnsOne(x => x.DateOfBirth);
        }
    }

    public static class PetDbContextExtensions
    {
        public static void AddPetDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PetDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Pet"));
            });
        }
        public static void EnsurePetDbIsCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<PetDbContext>();
            context.Database.EnsureCreated();
            context.Database.CloseConnection();
        }
    }
}