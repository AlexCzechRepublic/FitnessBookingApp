using System.ComponentModel.DataAnnotations;

namespace FitnessBookingApp.Models
{
    public class ProfileUpdateViewModel
    {
        [Required(ErrorMessage = "Jméno je povinné."), StringLength(50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Příjmení je povinné."), StringLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Ulice je povinná."), StringLength(100)]
        public string? Street { get; set; }

        [Required(ErrorMessage = "Číslo popisné je povinné."), StringLength(20)]
        public string? HouseNumber { get; set; }

        [Required(ErrorMessage = "PSČ je povinné.")]
        [RegularExpression(@"^\d{3}\s?\d{2}$", ErrorMessage = "PSČ musí být ve tvaru 12345 nebo 123 45.")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Město je povinné."), StringLength(100)]
        public string? City { get; set; }
    }
}