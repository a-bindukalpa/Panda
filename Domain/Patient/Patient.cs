using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Patient
{
    public string Id { get; set; }

    [Required(ErrorMessage = "NHS number is required.")]
    public string NhsNumber { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
    public required string Name { get; set; }

    [Required(ErrorMessage = "Date of birth is required.")]
    public required DateTime DateOfBirth { get; set; }


    [Required(ErrorMessage = "Postcode is required.")]
    public required string PostCode { get; set; }
}