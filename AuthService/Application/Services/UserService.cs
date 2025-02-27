﻿using Application.Interfaces;
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

        public UserService(IUserRepository repository, IValidator<RegisterViewModel> validator, UserManager<User> userManager)
        {
            this.repository = repository;
            this.validator = validator;
            this.userManager = userManager;
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
    }
}
