using System.Net;
using System.Net.Http.Json;
using Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace IntegrationTests;
public class AppointmentsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AppointmentsTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task PostAppointment_ValidAppointment_ReturnsCreated()
    {
        var appointment = new Appointment
        {
            Status = "Active",
            Time = DateTime.UtcNow.AddDays(1),
            Duration = "1h30m",
            Clinician = "Dr Smith",
            Department = "Cardiology",
            Postcode = "AB12 3CD"
        };

        var response = await _client.PostAsJsonAsync("/api/appointments", appointment);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var created = await response.Content.ReadFromJsonAsync<Appointment>();
        Assert.NotNull(created);
        Assert.Equal(appointment.Status, created.Status);
    }

    [Fact]
    public async Task PostAppointment_InvalidDuration_ReturnsBadRequest()
    {
        var appointment = new Appointment
        {
            Status = "Active",
            Time = DateTime.UtcNow.AddDays(1),
            Duration = "invalid",
            Clinician = "Dr Smith",
            Department = "Cardiology",
            Postcode = "AB12 3CD"
        };

        var response = await _client.PostAsJsonAsync("/api/appointments", appointment);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetAppointment_ReturnsAppointment()
    {
        var appointment = new Appointment
        {
            Status = "Active",
            Time = DateTime.UtcNow.AddDays(1),
            Duration = "1h",
            Clinician = "Dr Smith",
            Department = "Cardiology",
            Postcode = "AB12 3CD"
        };

        var postResponse = await _client.PostAsJsonAsync("/api/appointments", appointment);
        var created = await postResponse.Content.ReadFromJsonAsync<Appointment>();

        var getResponse = await _client.GetAsync($"/api/appointments/{created.Id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var fetched = await getResponse.Content.ReadFromJsonAsync<Appointment>();
        Assert.NotNull(fetched);
        Assert.Equal(created.Id, fetched.Id);
    }
}