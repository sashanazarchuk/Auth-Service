using Application.DTOs;
using Application.Interfaces;
using Application.IRepositories;
using Application.Models;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Security.Authentication;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IValidator<RegisterViewModel> validator;
        private readonly IJwtTokenService jwtService;

        public UserService(IUserRepository repository, IValidator<RegisterViewModel> validator, IJwtTokenService jwtService)
        {
            this.repository = repository;
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

            if (await repository.IsEmailExistsAsync(model.Email))
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

            return await repository.CreateUserAsync(user, model.Password);
        }


        public async Task<TokenDto> Login(LoginViewModel model)
        {

            var user = await repository.FindUserByEmailAsync(model.Email);
            if (user == null)
            {
                throw new AuthenticationException("Invalid email or password");
            }


            var isPasswordValid = await repository.ValidatePasswordAsync(user, model.Password);
            if (!isPasswordValid)
            {
                throw new AuthenticationException("Invalid email or password");
            }

            var jwtToken = jwtService.CreateAccessToken(user);
            var refreshToken = jwtService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await repository.UpdateUserAsync(user);

            return new TokenDto
            {
                AccessToken = jwtToken,
                RefreshToken = refreshToken
            };
        }


        public async Task RevokeToken(string userId)
        {
            var user = await repository.FindUserByIdAsync(userId);

            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null;

            await repository.UpdateUserAsync(user);
        }
    }
}