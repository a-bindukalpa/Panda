using Domain;
using System.ComponentModel.DataAnnotations;
using AwesomeAssertions;
using AwesomeAssertions.Execution;

namespace DomainTests;

public class AppointmentTests
{
    private static Appointment Sample(
        string id, string status, 
        DateTime time, string duration, string clinician,
        string department, string postcode) =>
        new()
        {
            Id = id,
            Status = status,
            Time = time,
            Duration = duration,
            Clinician = clinician,
            Department = department,
            Postcode = postcode
        };

    [Fact]
    public void Validation_Fails_ForInvalidDuration()
    {
        var appointment = Sample(
            "A2", "Confirmed", DateTime.Now,
            "1.5 hours", "Dr Smith", "Cardiology", "AB12 3CD");

        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(appointment, new ValidationContext(appointment), errors, true);

        using var _ = new AssertionScope();
        isValid.Should().BeFalse();
        errors.Should().Contain(r => r.ErrorMessage == "Duration must be in the format '1h30m', '1h', or '15m'.");
    }

    [Fact]
    public void Validation_Fails_ForNegativeOrZeroDuration()
    {
        var appointment = Sample(
            "A5", "Confirmed", DateTime.Now,
            "0", "Dr Smith", "Cardiology", "AB12 3CD");

        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(appointment, new ValidationContext(appointment), errors, true);

        using var _ = new AssertionScope();
        isValid.Should().BeFalse();
        errors.Should().Contain(r => r.ErrorMessage == "Duration must be in the format '1h30m', '1h', or '15m'.");
    }

    [Fact]
    public void Validation_Fails_ForTooShortClinicianName()
    {
        var appointment = Sample(
            "A3", "Confirmed", DateTime.Now,
            "60m", "A", "Cardiology", "AB12 3CD");

        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(appointment, new ValidationContext(appointment), errors, true);

        using var _ = new AssertionScope();
        isValid.Should().BeFalse();
        errors.Should().Contain(r => r.ErrorMessage == "Clinician name must be at least 2 characters.");
    }

    [Fact]
    public void Validation_Succeeds_ForValidData()
    {
        var appointment = Sample(
            "A4", "Active",
            new DateTime(2025, 7, 17, 14, 0, 0),
            "1h20m", "Dr Jones", "Neurology", "XY99 9ZZ");

        var errors = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(appointment, new ValidationContext(appointment), errors, true);

        using var _ = new AssertionScope();
        isValid.Should().BeTrue();
        errors.Should().BeEmpty();
    }
}