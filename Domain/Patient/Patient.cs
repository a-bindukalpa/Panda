using Panda.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain;

public class Patient
{
    private string _postcode;

    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required(ErrorMessage = "NHS number is required.")]
    public string NhsNumber { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    public required DateTime DateOfBirth { get; set; }


    [Required(ErrorMessage = "Postcode is required.")]
    public string PostCode
    {
        get => _postcode;
        set => _postcode = PostcodeUtil.CoerceOrInvalid(value);
    }

    public static bool IsValidNHSNumber(string nhsNumber)
    {
        if (string.IsNullOrWhiteSpace(nhsNumber) || nhsNumber.Length != 10 || !nhsNumber.All(char.IsDigit))
            return false;

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (nhsNumber[i] - '0') * (10 - i);
        }

        int remainder = sum % 11;
        int checkDigit = 11 - remainder;
        if (checkDigit == 11) checkDigit = 0;
        if (checkDigit == 10) return false;

        return checkDigit == (nhsNumber[9] - '0');
    }
}