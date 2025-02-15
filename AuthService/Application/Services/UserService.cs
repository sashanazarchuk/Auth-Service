using Application.Interfaces;
using Application.IRepositories;
using Application.Models;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository repository;
        public UserService(IUserRepository repository)
        {
            this.repository = repository;
        }
        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {

            if (string.IsNullOrEmpty(model.Email))
            {
                return IdentityResult.Failed(new IdentityError { Code = "InvalidEmail", Description = "Email is required." });
            }

            var emailExists = await repository.EmailExistsAsync(model.Email);
            if (emailExists)
            {
                return IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email is already registered." });
            }


            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                Country = model.Country
            };



            var result = await repository.CreateUserAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                return IdentityResult.Failed(new IdentityError { Code = "RegistrationFailed", Description = $"Registration failed: {errorMessage}" });
            }
            return result;
        }
    }
}
