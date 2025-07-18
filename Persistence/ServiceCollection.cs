using Domain;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence;

public static class ServiceCollection
{
    public static void AddPersistence(this IServiceCollection services)
    {
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<IMongoCollectionAdapter<Patient>, MongoCollectionAdapter<Patient>>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IMongoCollectionAdapter<Appointment>, MongoCollectionAdapter<Appointment>>();
    }
}