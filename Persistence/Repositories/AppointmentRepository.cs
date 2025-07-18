using Domain;
using MongoDB.Driver;

namespace Persistence;

public class AppointmentRepository(IMongoCollectionAdapter<Appointment> adapter) : IAppointmentRepository
{
    public Task<Appointment?> GetAsync(string id)
    {
        var filter = Builders<Appointment>.Filter.Eq(p => p.Id, id);
        return adapter.FindOneAsync(filter);
    }

    public Task AddAsync(Appointment appointment) => adapter.InsertOneAsync(appointment);

    public Task<bool> UpdateAsync(Appointment appointment)
    {
        var filter = Builders<Appointment>.Filter.Eq(p => p.Id, appointment.Id);
        return adapter.ReplaceOneAsync(filter, appointment);
    }
}