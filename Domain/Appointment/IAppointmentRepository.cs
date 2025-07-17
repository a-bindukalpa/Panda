using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Appointment
{
    public interface IAppointmentRepository
    {
        Task<IEnumerable<Appointment>> GetAllAsync();
        Task<Appointment?> GetByIdAsync(string id);
        Task AddAsync(Appointment appointment);
        Task<bool> UpdateAsync(Appointment appointment);
        Task<bool> DeleteAsync(string id);
    }
}
