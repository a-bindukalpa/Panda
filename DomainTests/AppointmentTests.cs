using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace DomainTests
{
    public class AppointmentTests
    {
        [Fact]
        public void Appointment_Properties_AssignCorrectly()
        {
            var appointment = new Appointment
            {
                Id = "A1",
                Status = "Confirmed",
                Time = new DateTime(2025, 7, 17, 10, 0, 0),
                Duration = "01:30",
                Clinician = "Dr Smith",
                Department = "Cardiology",
                Postcode = "AB12 3CD"
            };

            Assert.Equal("A1", appointment.Id);
            Assert.Equal("Confirmed", appointment.Status);
            Assert.Equal(new DateTime(2025, 7, 17, 10, 0, 0), appointment.Time);
            Assert.Equal("01:30", appointment.Duration);
            Assert.Equal("Dr Smith", appointment.Clinician);
            Assert.Equal("Cardiology", appointment.Department);
            Assert.Equal("AB12 3CD", appointment.Postcode);
        }

        [Fact]
        public void Appointment_Validation_Fails_WhenDurationFormatInvalid()
        {
            var appointment = new Appointment
            {
                Id = "A2",
                Status = "Confirmed",
                Time = DateTime.Now,
                Duration = "1.5 hours",
                Clinician = "Dr Smith",
                Department = "Cardiology",
                Postcode = "AB12 3CD"
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(appointment);

            bool isValid = Validator.TryValidateObject(appointment, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Duration must be in HH:mm format.");
        }

        [Fact]
        public void Appointment_Validation_Fails_WhenClinicianNameTooShort()
        {
            var appointment = new Appointment
            {
                Id = "A3",
                Status = "Confirmed",
                Time = DateTime.Now,
                Duration = "01:00",
                Clinician = "A",
                Department = "Cardiology",
                Postcode = "AB12 3CD"
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(appointment);

            bool isValid = Validator.TryValidateObject(appointment, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Clinician name must be at least 2 characters.");
        }

        [Fact]
        public void Appointment_Validation_Succeeds_WithValidData()
        {
            var appointment = new Appointment
            {
                Id = "A4",
                Status = "Confirmed",
                Time = new DateTime(2025, 7, 17, 14, 0, 0),
                Duration = "02:00",
                Clinician = "Dr Jones",
                Department = "Neurology",
                Postcode = "XY99 9ZZ"
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(appointment);

            bool isValid = Validator.TryValidateObject(appointment, context, results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}