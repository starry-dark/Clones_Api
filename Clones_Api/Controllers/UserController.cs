using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Enums;

namespace Clones_Api.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var result = await _userService.Register(request);
            return StatusCode(result.Code, result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _userService.Login(request);
            return StatusCode(result.Code, result);
        }

        [Authorize(Roles = nameof(Roles.Admin))]
        [HttpGet("users")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userService.ListUsers();
            return StatusCode(result.Code, result);
        }
    }
}
