using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PokerApp.Server.Interfaces;
using PokerApp.Server.Models;
using PokerApp.Server.Services;
using System.Runtime.InteropServices;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PokerApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        readonly IUserService _userService;
        readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userService.LoginAsync(loginRequest);
            if (user==null)
                return Unauthorized();
            var response = new LoginResponse();
            response.Token = _tokenService.CreateToken(user);
            response.Success = String.IsNullOrEmpty(response.Token) ? false : true;
            return Ok(response);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<IActionResult> SignUp([FromBody] User newUser)
        {
            var user = await _userService.SignUpAsync(newUser);
            if (user == null)
                return BadRequest();
            return Ok(user);
        }
    }
}