using Domain;
using MongoDB.Driver;

namespace Persistence.Repositories;

public class PatientRepository(IMongoCollectionAdapter<Patient> adapter) : IPatientRepository
{
    public Task<Patient?> GetAsync(string nhsNumber)
    {
        var filter = Builders<Patient>.Filter.Eq(p => p.NhsNumber, nhsNumber);
        return adapter.FindOneAsync(filter);
    }

    public Task AddAsync(Patient patient) => adapter.InsertOneAsync(patient);

    public Task<bool> UpdateAsync(Patient patient)
    {
        var filter = Builders<Patient>.Filter.Eq(p => p.NhsNumber, patient.NhsNumber);
        return adapter.ReplaceOneAsync(filter, patient);
    }

    public Task<bool> DeleteAsync(string nhsNumber)
    {
        var filter = Builders<Patient>.Filter.Eq(p => p.NhsNumber, nhsNumber);
        return adapter.DeleteOneAsync(filter);
    }
}