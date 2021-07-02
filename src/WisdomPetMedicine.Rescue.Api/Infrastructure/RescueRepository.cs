using System.Threading.Tasks;
using WisdomPetMedicine.Rescue.Domain.Entities;
using WisdomPetMedicine.Rescue.Domain.Repositories;
using WisdomPetMedicine.Rescue.Domain.ValueObjects;

namespace WisdomPetMedicine.Rescue.Api.Infrastructure
{
    public class RescueRepository : IRescueRepository
    {
        private readonly RescueDbContext dbContext;

        public RescueRepository(RescueDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAdopterAsync(Adopter adopter)
        {
            dbContext.Adopters.Add(adopter);
            await dbContext.SaveChangesAsync();
        }

        public async Task AddRescuedAnimalAsync(RescuedAnimal rescuedAnimal)
        {
            dbContext.RescuedAnimals.Add(rescuedAnimal);
            await dbContext.SaveChangesAsync();
        }

        public async Task<Adopter> GetAdopterAsync(AdopterId id)
        {
            return await dbContext.Adopters.FindAsync(id.Value);
        }

        public async Task<RescuedAnimal> GetRescuedAnimalAsync(RescuedAnimalId id)
        {
            return await dbContext.RescuedAnimals.FindAsync(id.Value);
        }

        public async Task UpdateAdopterAsync(Adopter adopter)
        {
            var adopterToUpdate = await dbContext.Adopters.FindAsync(adopter.Id);
            if (adopterToUpdate == null)
            {
                dbContext.Adopters.Add(adopter);
            }
            else
            {
                dbContext.Adopters.Update(adopter);
            }
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateRescuedAnimalAsync(RescuedAnimal rescuedAnimal)
        {
            var rescuedAnimalToUpdate = await dbContext.RescuedAnimals.FindAsync(rescuedAnimal.Id);
            if (rescuedAnimalToUpdate == null)
            {
                dbContext.RescuedAnimals.Add(rescuedAnimal);
            }
            else
            {
                dbContext.RescuedAnimals.Update(rescuedAnimal);
            }
            await dbContext.SaveChangesAsync();
        }
    }
}