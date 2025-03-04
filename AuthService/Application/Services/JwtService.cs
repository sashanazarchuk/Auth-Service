using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JwtTokenService : IJwtTokenService
    {

        private readonly IConfiguration configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim("email", user.Email),
                new Claim("country", user.Country),
            };

            var jwtIssuer = configuration["JwtSettings:Issuer"];
            var jwtAudience = configuration["JwtSettings:Audience"];
            var signinKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Secret"]));
            var signinCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256);


            var jwtToken = new JwtSecurityToken(
                
                issuer: jwtIssuer,
                audience: jwtAudience,
                signingCredentials: signinCredentials,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2)

            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
