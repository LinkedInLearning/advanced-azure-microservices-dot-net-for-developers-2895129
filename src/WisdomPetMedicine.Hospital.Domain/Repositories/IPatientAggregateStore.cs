using System.Threading.Tasks;
using WisdomPetMedicine.Hospital.Domain.Entities;
using WisdomPetMedicine.Hospital.Domain.ValueObjects;

namespace WisdomPetMedicine.Hospital.Domain.Repositories
{
    public interface IPatientAggregateStore
    {
        Task SaveAsync(Patient patient);
        Task<Patient> LoadAsync(PatientId patient);
    }
}