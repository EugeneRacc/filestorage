using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    //[ApiVersionNeutral]
    [Produces("application/json")]
    [Route("api/{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAdminService _adminService;
        public AdminController(IUnitOfWork uow, IMapper mapper, IConfiguration configuration)
        {
            _userService = new UserService(uow, mapper, configuration);
            _adminService = new AdminService(uow, mapper, configuration);
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
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<IEnumerable<UserModel>>> Get([FromQuery] string? sort, string? name)
        {
            IEnumerable<UserModel> customers;
            try
            {
                customers = await _adminService.GetAllUsersAsync(sort, name);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(customers);
        }

        [HttpGet("users/{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<ActionResult<UserModel>> GetById(string id)
        {
            UserModel customers;
            try
            {
                customers = await _adminService.GetByIdAsync(int.Parse(id));
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(customers);
        }

        [HttpGet("users/{id}/files")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetFilesByName(string id, [FromQuery] string? sort, string? name)
        {
            IEnumerable<FileModel> resultFiles;
            try
            {
                resultFiles = await _adminService.GetUserFilesAsync(int.Parse(id), sort, name);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok(resultFiles);
        }

        [HttpPut("users")]
        public async Task<ActionResult> Update([FromBody] UserForUpdateModel value)
        {

            try
            {
                await _adminService.UpdateUserAsync(value);
            }
            catch (Exception e)
            {
                return BadRequest("Not found");
            }

            return Ok("Success");
        }

    }
}
