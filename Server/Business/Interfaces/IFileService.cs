using Business.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    /// <summary>
    /// Interface with an extension of functionality for working with files
    /// </summary>
    /// <seealso cref="Business.Interfaces.ICrud&lt;Business.Models.FileModel&gt;" />
    public interface IFileService : ICrud<FileModel>
    {
        /// <summary>
        /// Creates the dir.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>FileModel of created dir</returns>
        public Task<FileModel> CreateDir(FileModel model);
        /// <summary>
        /// Gets the files by parent identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="sortType">Type of the sort.</param>
        /// <returns>Return IEnumerable of files that are in the dir with needed Id</returns>
        public Task<IEnumerable<FileModel>> GetFilesByParentIdAsync(int userId, int? parentId, string? sortType);
        /// <summary>
        /// Gets the files by user identifier asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sortType">Type of the sort.</param>
        /// <returns>IEnumerable of Files that are owned by user</returns>
        public Task<IEnumerable<FileModel>> GetFilesByUserIdAsync(int userId, string? sortType);
        /// <summary>
        /// Gets the name of the files by.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Files with needed name of user</returns>
        public Task<IEnumerable<FileModel>> GetFilesByName(int userId, string fileName);
        /// <summary>
        /// Uploads the file asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="formFile">The form file.</param>
        /// <returns>FileModel of new file</returns>
        public Task<FileModel> UploadFileAsync(int userId, string? parentId, IFormFile formFile);
        /// <summary>
        /// Downloads the file asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="fileId">The file identifier.</param>
        /// <returns>Model with all info for download file in web api</returns>
        public Task<DownloadFileModel> DownloadFileAsync(int userId, int fileId);
        /// <summary>
        /// Downloads the file by access link asynchronous.
        /// </summary>
        /// <param name="accessLink">The access link.</param>
        /// <returns>Model with all info for download file in web api</returns>
        public Task<DownloadFileModel> DownloadFileByAccessLinkAsync(string accessLink);
        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="userId">The user identifier.</param>
        /// <returns>Delete the file by Id</returns>
        public Task DeleteAsync(int modelId, int userId);
    }
}