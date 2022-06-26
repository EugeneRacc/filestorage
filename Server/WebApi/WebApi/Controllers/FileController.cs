using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/{version:apiVersion}")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        public FileController(IUnitOfWork uow, IMapper mapper, IConfiguration configuration)
        {
            _fileService = new FileService(uow, mapper, configuration);
            _userService = new UserService(uow, mapper, configuration);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateDirectory([FromBody] FileModel model)
        {
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                model.UserId = user.Result;
                await _fileService.CreateDir(model);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetFiles(string id)
        {
            IEnumerable<FileModel> resultFiles;
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                resultFiles = await _fileService.GetFilesByParentIdAsync(user.Result, int.Parse(id));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok(resultFiles);
        }
    }
}
