using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Appointment
{
    public class Appointment
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Time is required.")]
        public DateTime Time { get; set; }

        [Required(ErrorMessage = "Duration is required.")]
        [RegularExpression(@"^\d{1,2}:\d{2}$", ErrorMessage = "Duration must be in HH:mm format.")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Clinician is required.")]
        [MinLength(2, ErrorMessage = "Clinician name must be at least 2 characters.")]
        public string Clinician { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public string Department { get; set; }

        [Required(ErrorMessage = "Postcode is required.")]
        public string Postcode { get; set; }
    }
}
