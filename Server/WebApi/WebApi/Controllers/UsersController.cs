using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace WebApi.Controllers
{
    /// <summary>
    /// Controller for working with users
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUnitOfWork uow, IMapper mapper, IConfiguration configuration)
        {
            _userService = new UserService(uow, mapper, configuration);
        }
        /// <summary>
        /// Gets the User by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>User</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserModel>> GetById(int id)
        {
            UserModel resultCustomer;
            try
            {
                resultCustomer = await _userService.GetByIdAsync(id);
                if (resultCustomer == null)
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(resultCustomer);
        }
        /// <summary>
        /// Adds new User.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>Info about user</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Add([FromBody] UserModel value)
        {
            try
            {
                await _userService.AddAsync(value);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(Add), new { id = value.Id }, value);
        }
        /// <summary>
        /// Updates the specified user with needed ID.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <returns>String with success</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(int Id, [FromBody] UserModel value)
        {

            try
            {
                await _userService.UpdateAsync(value);
            }
            catch (Exception)
            {
                return BadRequest();
            }

            return Ok("Success");
        }
        /// <summary>
        /// Deletes the User with specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>String about success</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _userService.DeleteAsync(id);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok("Success");
        }
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
                    RoleName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Role)?.Value
                };
            }
            return null;
        }

    }
}