using System.ComponentModel.DataAnnotations;

namespace Application.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [RegularExpression(@"^\+([1-9]{1,4})\d{6,14}$", ErrorMessage = "Please enter a valid phone number with the country code (e.g. +380293365932)")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

       [Required(ErrorMessage = "This field is required")]
       [Range(18, int.MaxValue, ErrorMessage = "Age must be from 18")]
        public int Age { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Country { get; set; }
    }
}
