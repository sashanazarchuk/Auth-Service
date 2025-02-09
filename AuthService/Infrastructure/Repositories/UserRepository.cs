using Application.IRepositories;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> userManager;

        public UserRepository(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }
        public async Task<IdentityResult> RegisterAsync(User user, string password)
        {
            if (string.IsNullOrEmpty(user.Email))
            {
                return IdentityResult.Failed(new IdentityError { Code = "InvalidEmail", Description = "Email is required." });
            }

            var existingUser = await userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError { Code = "DuplicateEmail", Description = "Email is already registered." });

            return await userManager.CreateAsync(user, password);
        }
    }
}
