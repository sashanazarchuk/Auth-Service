using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int Age { get; set; }
        public string Country { get; set; }
    }
}