using System.Threading.Tasks;
using WisdomPetMedicine.Pet.Domain.ValueObjects;

namespace WisdomPetMedicine.Pet.Domain.Repositories
{
    public interface IPetRepository
    {
        Task<Entities.Pet> GetAsync(PetId id);
        Task AddAsync(Entities.Pet pet);
        Task UpdateAsync(Entities.Pet pet);
    }
}