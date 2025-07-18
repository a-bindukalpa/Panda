using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain;

public interface IAppointmentRepository
{
    Task<Appointment?> GetAsync(string id);
    Task AddAsync(Appointment appointment);
    Task<bool> UpdateAsync(Appointment appointment);
}