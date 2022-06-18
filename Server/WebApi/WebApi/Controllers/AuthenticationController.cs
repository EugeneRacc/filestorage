using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebApi.Controllers
{

    [Route("filestorage/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthenticationController(IUnitOfWork uow, IMapper mapper)
        {
            _userService = new UserService(uow, mapper);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Add([FromBody] UserModel value)
        {
            try
            {
                await _userService.AddAsync(value);
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(400));
            }
            return CreatedAtAction(nameof(Add), value);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LogIn([FromBody] UserModel value)
        {
            bool result = false;
            try
            {
                result = await _userService.LogInAsync(value);
            }
            catch (Exception)
            {
                return BadRequest(StatusCode(400));
            }
            return CreatedAtAction(nameof(LogIn), result);
        }


    }
}