namespace Domain;

public interface IPatientRepository
{        
    Task<Patient?> GetAsync(string nhsNumber);
    Task AddAsync(Patient patient);
    Task<bool> UpdateAsync(Patient patient);
    Task<bool> DeleteAsync(string nhsNumber);
}