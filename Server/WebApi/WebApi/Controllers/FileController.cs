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
    /// <summary>
    /// Controller for working with Files
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.ControllerBase" />
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
        /// <summary>
        /// Creates the directory for user.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>File Model of new directory</returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
        /// <summary>
        /// Gets the files after sorting.
        /// </summary>
        /// <param name="sortType">Type of the sort.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Get file with needed File Name
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>File</returns>
        [HttpGet("filter")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetFilesByName([FromQuery] string? fileName)
        {
            IEnumerable<FileModel> resultFiles;
            try
            {
                if (fileName == null)
                {
                    await GetFiles(null);
                    return Ok();
                }
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                resultFiles = await _fileService.GetFilesByName(user.Result, fileName);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok(resultFiles);
        }
        /// <summary>
        /// Gets the files that are saved in directory with parent identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="sortType">Type of the sort.</param>
        /// <returns>List of files</returns>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

        /// <summary>
        /// Uploads the file into db and locally server.
        /// </summary>
        /// <param name="uploadedFile">The uploaded file.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <returns>Info about new file</returns>
        [HttpPost("upload"), DisableRequestSizeLimit]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile uploadedFile, [FromForm]string? parentId)
        {
            FileModel addedlFile;
            try
            {
                string authHeader = Request.Headers["Authorization"];
                var user = new AuthenticationService(_userService).GetUserIdByToken(authHeader);
                addedlFile = await _fileService.UploadFileAsync(user.Result, parentId, uploadedFile);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            return Ok(addedlFile);
        }
        /// <summary>
        /// Downloads the file.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Blob for downloading file</returns>
        [HttpGet("download"), DisableRequestSizeLimit]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
                    FileDownloadName = resultDownloadFile.FileName + "." + resultDownloadFile.Type
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Downloads the file by access link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <returns>Blob for downloading file</returns>
        [HttpGet("share"), DisableRequestSizeLimit]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult DownloadFileByLink([FromQuery] string link)
        {
            IEnumerable<FileModel> resultFiles;
            DownloadFileModel resultDownloadFile;
            try
            {
                resultDownloadFile = _fileService.DownloadFileByAccessLinkAsync(link).Result;
                HttpContext.Response.Headers.Add("x-my-custom-header", $"{resultDownloadFile.FileName}.{resultDownloadFile.Type}");
                return Ok(new FileContentResult(resultDownloadFile.Memory, resultDownloadFile.Extension)
                {
                   FileDownloadName = resultDownloadFile.FileName + "." + resultDownloadFile.Type
                });
                return Ok(resultDownloadFile.FileName);
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        /// <summary>
        /// Deletes the specified file by identifier.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <returns>String about success</returns>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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

