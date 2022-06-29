using AutoMapper;
using Business.Exceptions;
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
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/{version:apiVersion}/[controller]")]
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
            FileModel result;
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                model.UserId = user.Result;
                result = await _fileService.CreateDir(model);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetFiles([FromQuery] string? sortType)
        {
            IEnumerable<FileModel> resultFiles;
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                resultFiles = await _fileService.GetFilesByUserIdAsync(user.Result, sortType);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok(resultFiles);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetFilesWithParentId(string id, [FromQuery] string? sortType)
        {
            IEnumerable<FileModel> resultFiles;
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                resultFiles = await _fileService.GetFilesByParentIdAsync(user.Result, int.Parse(id), sortType);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok(resultFiles);
        }

        
        [HttpPost("upload"), DisableRequestSizeLimit]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile uploadedFile, [FromForm]string? parentId)
        {
            FileModel addedlFile;
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                addedlFile = await _fileService.UploadFileAsync(user.Result, parentId, uploadedFile);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
            return Ok(addedlFile);
        }

        [HttpGet("download"), DisableRequestSizeLimit]
        [Authorize]
        public FileContentResult DownloadFile([FromQuery] string id)
        {
            IEnumerable<FileModel> resultFiles;
            DownloadFileModel resultDownloadFile;
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var userId = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                resultDownloadFile = _fileService.DownloadFileAsync(userId.Result, int.Parse(id)).Result;
                return new FileContentResult(resultDownloadFile.Memory, resultDownloadFile.Extension)
                {
                    FileDownloadName = resultDownloadFile.FileName
                };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] string modelId)
        {
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var userId = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                await _fileService.DeleteAsync(int.Parse(modelId),userId.Result);
                
            }
            catch (Exception e)
            {
                return BadRequest(error: 400);
            }
            return Ok("File was deleted");
        }
    }

}

