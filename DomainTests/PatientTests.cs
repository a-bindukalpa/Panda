using Domain;
using Domain.Patient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace DomainTests
{
    public class PatientTests
    {
        [Fact]
        public void Patient_Properties_AssignCorrectly()
        {
            var patient = new Patient
            {
                Id = "123",
                PatientNHSNumber = 4567890123,
                Name = "John Doe",
                DateOfBirth = new DateTime(1980, 1, 1),
                PostCode = "AB12 3CD"
            };

            Assert.Equal("123", patient.Id);
            Assert.Equal(4567890123, patient.PatientNHSNumber);
            Assert.Equal("John Doe", patient.Name);
            Assert.Equal(new DateTime(1980, 1, 1), patient.DateOfBirth);
            Assert.Equal("AB12 3CD", patient.PostCode);
        }

        [Fact]
        public void Patient_Validation_Fails_WhenNameTooShort()
        {
            var patient = new Patient
            {
                Id = "123",
                PatientNHSNumber = 4567890123,
                Name = "A",
                DateOfBirth = new DateTime(1980, 1, 1),
                PostCode = "AB12 3CD"
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(patient);

            bool isValid = Validator.TryValidateObject(patient, context, results, true);

            Assert.False(isValid);
            Assert.Contains(results, r => r.ErrorMessage == "Name must be at least 2 characters.");
        }

        [Fact]
        public void Patient_Validation_Succeeds_WithValidData()
        {
            var patient = new Patient
            {
                Id = "123",
                PatientNHSNumber = 4567890123,
                Name = "Jane Doe",
                DateOfBirth = new DateTime(1990, 5, 15),
                PostCode = "XY99 9ZZ"
            };

            var results = new List<ValidationResult>();
            var context = new ValidationContext(patient);

            bool isValid = Validator.TryValidateObject(patient, context, results, true);

            Assert.True(isValid);
            Assert.Empty(results);
        }
    }
}