using Domain;
using MongoDB.Driver;
using Persistence;
using Service;

namespace Panda.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddPersistence();
        builder.Services.AddSingleton<IMongoDatabase>(sp =>

        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("MongoDb");
            var databaseName = configuration["MongoDbSettings:DatabaseName"];
            var client = new MongoClient(connectionString);
            return client.GetDatabase(databaseName);
        });

        builder.Services.AddScoped<IMongoCollection<Patient>>(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<Patient>("Patients");
        });

        builder.Services.AddScoped<IMongoCollection<Appointment>>(sp =>
        {
            var database = sp.GetRequiredService<IMongoDatabase>();
            return database.GetCollection<Appointment>("Appointments");
        });

        builder.Services.AddScoped<IAppointmentService, AppointmentService>();

        var app = builder.Build();
        app.UseSwagger();
        app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "Panda API V1"); });
        
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}