using System.ComponentModel.DataAnnotations;

namespace FitnessBookingApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Heslo musí mít alespoň 8 znaků.")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
