using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var result = await service.RegisterAsync(model);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok(new { Message = "User registered successfully" });
        }
    }
}
