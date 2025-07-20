using Domain;
using System.ComponentModel.DataAnnotations;

namespace Service;

public class AppointmentService(IAppointmentRepository repository) : IAppointmentService
{
    public Task<Appointment?> GetAsync(string id) => repository.GetAsync(id);

    public async Task AddAsync(Appointment appointment)
    {
        Validate(appointment);
        await repository.AddAsync(appointment);
    }

    public async Task<bool> UpdateAsync(Appointment appointment)
    {
        try {
            Validate(appointment);

            var existing = await repository.GetAsync(appointment.Id);
            CheckExistingAppointment(appointment, existing); 
            return await repository.UpdateAsync(appointment);
        }
        catch(Exception)
        {
            return false;
        }
    }

    private static void Validate(Appointment appointment)
    {
        var context = new ValidationContext(appointment);
        Validator.ValidateObject(appointment, context, validateAllProperties: true);
        if (!IsValidStatus(appointment.Status))
            throw new ValidationException("Invalid status value.");
    }

    private static void CheckExistingAppointment(Appointment appointment, Appointment? existing)
    {
        if (existing == null)        
            throw new ValidationException("The appointment for the given id does not exist");
        
        if(appointment.Status != AppointmentStatus.Cancelled.ToString() && existing.Status == AppointmentStatus.Cancelled.ToString())
            throw new ValidationException("Cancelled appointments cannot be reinstated.");
    }

    private static bool IsValidStatus(string status)
    {
        return Enum.TryParse<AppointmentStatus>(status, true, out var parsed) &&
               Enum.IsDefined(typeof(AppointmentStatus), parsed);
    }
}