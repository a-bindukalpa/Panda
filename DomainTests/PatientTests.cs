using Domain;
using System.ComponentModel.DataAnnotations;
using AwesomeAssertions;
using AwesomeAssertions.Execution;

namespace DomainTests;

public class PatientTests
{
    private Patient NewPatient(string id, string nhs, string name, DateTime dob, string postcode) =>
        new()
        {
            Id = id,
            NhsNumber = nhs,
            Name = name,
            DateOfBirth = dob,
            PostCode = postcode
        };

    [Fact]
    public void Patient_Validation_Fails_WhenNameTooShort()
    {
        var patient = NewPatient("123", "4567890123", "A", new DateTime(1980, 1, 1), "AB12 3CD");

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(patient, new ValidationContext(patient), results, true);

        using var _ = new AssertionScope();
        isValid.Should().BeFalse();
        results.Should().Contain(r => r.ErrorMessage == "Name must be at least 2 characters.");
    }

    [Fact]
    public void Patient_Validation_Succeeds_WithValidData()
    {
        var patient = NewPatient(
            "123", "4567890123", "Jane Doe",
            new DateTime(1990, 5, 15), "XY99 9ZZ");

        var results = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(patient, new ValidationContext(patient), results, true);

        using var _ = new AssertionScope();
        isValid.Should().BeTrue();
        results.Should().BeEmpty();
    }

    [Theory]
    [InlineData("SW1A 1AA", "SW1A 1AA")]
    [InlineData("sw1a1aa", "SW1A 1AA")]
    [InlineData("INVALID", "INVALID")]
    [InlineData("", "INVALID")]
    public void Patient_Postcode_ValidatesAndNormalizes(string input, string expected)
    {
        var patient = new Patient { NhsNumber = "1234567890", Name = "Test", DateOfBirth = DateTime.Today, PostCode = input };
        Assert.Equal(expected, patient.PostCode);
    }
}