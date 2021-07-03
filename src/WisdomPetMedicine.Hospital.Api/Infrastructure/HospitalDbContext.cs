using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WisdomPetMedicine.Hospital.Api.IntegrationEvents;

namespace WisdomPetMedicine.Hospital.Api.Infrastructure
{
    public class HospitalDbContext : DbContext
    {
        public HospitalDbContext(DbContextOptions<HospitalDbContext> options) : base(options) { }
        public DbSet<PetTransferredToHospitalIntegrationEvent> PatientsMetadata { get; set; }
    }

    public static class HospitalDbContextExtensions
    {
        public static void AddHospitalDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HospitalDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Hospital"));
            });
        }
        public static void EnsureHospitalDbIsCreated(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<HospitalDbContext>();
            context.Database.EnsureCreated();
            context.Database.CloseConnection();
        }
    }
}