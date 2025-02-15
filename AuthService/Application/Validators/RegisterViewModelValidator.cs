using Application.IRepositories;
using Application.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class RegisterViewModelValidator : AbstractValidator<RegisterViewModel>
    {
        public RegisterViewModelValidator(IUserRepository userRepository)
        {

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("This field is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(async (email, cancellation) => !await userRepository.EmailExistsAsync(email))
                .WithMessage("Email is already registered.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("This field is required")
                .Matches(@"^\+(\d{1,4})\d{9}$")
                .WithMessage("Please enter a valid phone number with the country code (e.g. +380293365932)");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("This field is required");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("This field is required")
                .Equal(x => x.Password)
                .WithMessage("The password and confirmation password do not match");

            RuleFor(x => x.Age)
                .GreaterThanOrEqualTo(18).WithMessage("Age must be from 18");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("This field is required");
        }
    }
}
