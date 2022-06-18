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

namespace WebApi.Controllers
{
    [Produces("application/json")]
    [Route("filestorage/[controller]")]
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
        /// GET /filestorage/users
        /// </remarks>
        /// <returns>Returns UserModel</returns>
        /// <response code="200">Success</response>
        /// <response code="401">If the user is unauthorized</response>
        [HttpGet]
        [Authorize]
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