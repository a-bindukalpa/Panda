using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Patient
{
    public interface IPatientRepository
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByNHSNumberAsync(int nhsNumber);
        Task AddAsync(Patient patient);
        Task<bool> UpdateAsync(Patient patient);
        Task<bool> DeleteAsync(int nhsNumber);
    }
}