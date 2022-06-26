using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Controllers
{

    [Route("api/{version:apiVersion}")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly IUserService _userService;
        public AuthenticationController(IUnitOfWork uow, IMapper mapper, IConfiguration config)
        {
            _configuration = config;
            _userService = new UserService(uow, mapper, config);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Add([FromBody] UserModel value)
        {
            try
            {
                await _userService.AddAsync(value);
            }
            catch (Exception e)
            {
                return BadRequest(StatusCode(400));
            }
            return CreatedAtAction(nameof(Add), value);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] UserLogin value)
        {
            var user = Authenticate(value); 
            if(user != null)
            {
                var token = Generate(user.Result);
                user.Result.Password = null;
                return Ok(new { token, user = user.Result });
            }
            return NotFound("User not found");
        }
        [AllowAnonymous]
        [HttpGet("auth")]
        public async Task<ActionResult> LogInByToken() 
        {
            string authHeader = Request.Headers["Authorization"];
            var user = new AuthenticationService(_userService).GetUserByToken(authHeader);
            if (user != null)
            {
                var token = Generate(user.Result);
                user.Result.Password = null;
                
                return Ok(new { token, user = user.Result });
            }
            return NotFound("User not found");
        }
        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleName)
            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<UserModel> Authenticate(UserLogin value)
        {
            var currentUser = await _userService.GetByUserCredentials(value);
            if(currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}