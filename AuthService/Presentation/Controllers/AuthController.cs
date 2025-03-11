using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService service;

        public AuthController(IUserService service)
        {
            this.service = service;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            var result = await service.RegisterUserAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "User registered successfully" });
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            try
            {
                var token = await service.Login(model);
                return Ok(token);
            }
            catch (AuthenticationException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

 
        [HttpPost("Revoke"), Authorize]
        public async Task<IActionResult> Revoke()
        {
            var userId = User.FindFirstValue("userId");
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not found");
            }

            await service.RevokeToken(userId);
            return NoContent();
        }
    }
}
