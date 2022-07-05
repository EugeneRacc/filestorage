using Business.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{

    public interface IFileService : ICrud<FileModel>
    {
        public Task<FileModel> CreateDir(FileModel model);
        public Task<IEnumerable<FileModel>> GetFilesByParentIdAsync(int userId, int? parentId, string? sortType);
        public Task<IEnumerable<FileModel>> GetFilesByUserIdAsync(int userId, string? sortType);
        public Task<IEnumerable<FileModel>> GetFilesByName(int userId, string fileName);
        public Task<FileModel> UploadFileAsync(int userId, string? parentId, IFormFile formFile);
        public Task<DownloadFileModel> DownloadFileAsync(int userId, int fileId);
        public Task<DownloadFileModel> DownloadFileByAccessLinkAsync(string accessLink);
        public Task DeleteAsync(int modelId, int userId);
    }
}