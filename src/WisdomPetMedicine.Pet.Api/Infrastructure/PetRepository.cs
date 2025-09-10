using WisdomPetMedicine.Pet.Domain.Repositories;
using WisdomPetMedicine.Pet.Domain.ValueObjects;

namespace WisdomPetMedicine.Pet.Api.Infrastructure;

public class PetRepository(PetDbContext petDbContext) : IPetRepository
{
    public async Task<Domain.Entities.Pet> GetAsync(PetId id)
    {
        return await petDbContext.Pets.FindAsync((Guid)id);
    }

    public async Task AddAsync(Domain.Entities.Pet pet)
    {
        petDbContext.Pets.Add(pet);
        await petDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Domain.Entities.Pet pet)
    {
        petDbContext.Pets.Update(pet);
        await petDbContext.SaveChangesAsync();
    }
}