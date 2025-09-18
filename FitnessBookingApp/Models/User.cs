using System.ComponentModel.DataAnnotations;
using FitnessBookingApp.Validators;

namespace FitnessBookingApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(3, ErrorMessage = "Uživatelské jméno musí mít alespoň 3 znaky.")]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string Role { get; set; } = "User";

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

        [Required(ErrorMessage = "Telefon je povinný.")]
        [CzSkPhone]
        public string? PhoneNumber { get; set; }

        public List<TrainingRegistration> TrainingRegistrations { get; set; } = new();

        public decimal Balance { get; set; } // vypočítané z entry
        public ICollection<Entry> Entries { get; set; }
    }
}