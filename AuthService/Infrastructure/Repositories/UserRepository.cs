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
        public async Task<bool> EmailExistsAsync(string email)
        {
            return await userManager.FindByEmailAsync(email) != null;
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await userManager.CreateAsync(user, password);
        }
    }
}
