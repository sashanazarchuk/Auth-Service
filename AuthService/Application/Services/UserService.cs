using Application.Interfaces;
using Application.IRepositories;
using Application.Models;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
    public class UserService : IUserService
    {
        public readonly IUserRepository repository;
        private readonly IValidator<RegisterViewModel> validator;
        private readonly UserManager<User> userManager;
        private readonly IJwtTokenService jwtService;

        public UserService(IUserRepository repository, IValidator<RegisterViewModel> validator, UserManager<User> userManager, IJwtTokenService jwtService)
        {
            this.repository = repository;
            this.userManager = userManager;
            this.validator = validator;
            this.jwtService = jwtService;
        }


        public async Task<IdentityResult> RegisterUserAsync(RegisterViewModel model)
        {

            var validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new IdentityError { Code = e.ErrorCode, Description = e.ErrorMessage });
                return IdentityResult.Failed(errors.ToArray());
            }

            if (await repository.EmailExistsAsync(model.Email))
            {
                return IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email already in use." });
            }

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Age = model.Age,
                Country = model.Country
            };

            return await userManager.CreateAsync(user, model.Password);
        }


        public async Task<string> Login(LoginViewModel model)
        {
        
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

       
            var isPasswordValid = await userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                throw new InvalidOperationException("Invalid email or password");
            }

           
            return jwtService.CreateToken(user);
        }
    }
}