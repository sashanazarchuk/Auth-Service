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
        Task<bool> EmailExistsAsync(string email);
        Task<IdentityResult> CreateUserAsync(User user, string password);
    }
}
