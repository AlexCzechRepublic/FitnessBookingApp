using System.ComponentModel.DataAnnotations;

namespace FitnessBookingApp.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public string Username { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public string Role { get; set; } = "User";

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        // NOVÉ
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
        public string? PostalCode { get; set; }
        public string? City { get; set; }
        public string? PhoneNumber { get; set; }

        public List<TrainingRegistration> TrainingRegistrations { get; set; } = new();
    }

}
