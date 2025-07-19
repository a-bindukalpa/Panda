using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;

namespace Service;

public interface IAppointmentService
{
    Task<Appointment?> GetAsync(string id);
    Task AddAsync(Appointment appointment);
    Task<bool> UpdateAsync(Appointment appointment);
}