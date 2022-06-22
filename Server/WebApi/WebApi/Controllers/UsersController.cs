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

namespace WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUnitOfWork uow, IMapper mapper)
        {
            _userService = new UserService(uow, mapper);
        }
        /// <summary>
        /// Gets all UserModels.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// GET /api/{version}/users
        /// </remarks>
        /// <returns>Returns UserModel</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
            
        public async Task<ActionResult<IEnumerable<UserModel>>> Get()
        {
            IEnumerable<UserModel> customers;
            try
            {
                customers = await _userService.GetAllAsync();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(customers);
        }
        [HttpGet("Admins")]
        [Authorize(Roles = "Admin")]
        public ActionResult AdminEndpoint()
        {
            var currentUser = GetCurrentUser();
            return Ok($"Hi {currentUser.Email}, you are an {currentUser.RoleName}");
        }

        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if(identity != null)
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

        [HttpGet("{id}")]
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

        [HttpPost]
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

        [HttpPut("{id}")]
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

            return Ok();
        }

        [HttpDelete("{id}")]
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
            return Ok();
        }

    }
}