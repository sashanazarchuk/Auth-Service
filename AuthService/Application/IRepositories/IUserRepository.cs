using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.IRepositories
{
    public interface IUserRepository
    {
        Task<bool> IsEmailExistsAsync(string email);
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User> FindUserByEmailAsync(string email);
        Task<User> FindUserByIdAsync(string userId);
        Task<bool> ValidatePasswordAsync(User user, string password);
        Task UpdateUserAsync(User user);

    }
}