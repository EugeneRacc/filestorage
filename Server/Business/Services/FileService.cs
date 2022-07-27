using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Business.PasswordHash;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{
    /// <summary>
    /// Class with all business logic for working with files
    /// </summary>
    /// <seealso cref="Business.Interfaces.IFileService" />
    public class FileService : IFileService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public FileService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration)
        {
            _db = uow;
            _mapper = mapper;
            _configuration = configuration;
            _userService = new UserService(uow, mapper, configuration);
        }

        public async Task<IEnumerable<FileModel>> GetAllAsync()
        {
                var files = await _db.FileRepository.GetAllWithDetailsAsync();
                var mappedfiles = _mapper.Map<IEnumerable<Data.Entities.File>, IEnumerable<FileModel>>(files);
                return mappedfiles;
            
        }

        public async Task<FileModel> GetByIdAsync(int id)
        {
            try
            {
                var file = await _db.FileRepository.GetByIdAsync(id);
                var mappedfile = _mapper.Map<Data.Entities.File, FileModel>(file);
                return mappedfile;
            }
            catch (NullReferenceException)
            {
                throw new FileStorageException("No such a file");
            }
        }
        public async Task<FileModel> GetByModelAsync(FileModel model)
        {
            var files = await GetAllAsync();
            var result = files.FirstOrDefault(x => x.Name == model.Name && x.UserId == model.UserId && x.Type == model.Type 
            && x.ParentId == model.ParentId && x.Path == model.Path);
            if(result == null)
            {
                throw new FileStorageException("No such a file");
            }
            return result;
        }

        public async Task AddAsync(FileModel model)
        {
            if (model == null)
            {
                throw new FileStorageException($"The customer cannot be null {nameof(model)}");
            }
            var file = _mapper.Map<FileModel, File>(model);
            await _db.FileRepository.AddAsync(file);
            await _db.SaveAsync();
        }

        public async Task UpdateAsync(FileModel model)
        {
            if (model == null)
            {
                throw new FileStorageException($"The user cannot be null {nameof(model)}");
            }
            _db.FileRepository.Update(_mapper.Map<File>(model));
            await _db.SaveAsync();
        }

        public async Task DeleteAsync(int modelId, int userId)
        {
            var fileToDelete = await GetByIdAsync(modelId);
            if(fileToDelete == null || fileToDelete.UserId != userId)
            {
                throw new FileStorageException("User or File with such an id doesn't exist");
            }
            string path = GetPath(userId, fileToDelete.Path);
            if(DeleteLocalFiles(path, fileToDelete.Type) == false)
            {
                throw new FileStorageException("Can't delete this file");
            }
            var user = await _userService.GetByIdWithNoTrackingAsync(userId);
            user.UsedDiskSpace = (long.Parse(user.UsedDiskSpace ?? "0") - long.Parse(fileToDelete.Size ?? "0")).ToString();
            await _db.FileRepository.DeleteByIdAsync(fileToDelete.Id);
            await _userService.UpdateAsync(user);
            await _db.SaveAsync();
        }
        public async Task DeleteAsync(int modelId)
        {
            await _db.FileRepository.DeleteByIdAsync(modelId);
            await _db.SaveAsync();
        }
        public async Task<FileModel> CreateDir(FileModel model)
        {
            File parentFile = null;
            model.AccessLink = MD5Hash.GetMD5Hash(model.Name + model.Type+ ";" + model.ParentId);
            if (model.ParentId != null) {
                parentFile = await _db.FileRepository.GetByIdAsync((int)model.ParentId);
            }
            if(parentFile == null)
            {
                model.Path = model.Name;
                await AddAsync(model);
                CreateDirWithAllInfo(model);
                try
                {
                    return await GetByModelAsync(model);
                }
                catch (Exception)
                {

                    throw new FileStorageException($"File not created {model.Name}");
                }
            }
            else
            {
                model.Path = @$"{parentFile.Path}\{model.Name}";
                try
                {
                    CreateDirWithAllInfo(model);
                    
                }
                catch (Exception)
                {
                    throw new FileStorageException($"File already exist {model.Name}");
                }
                await AddAsync(model);
                await _db.SaveAsync();
                try
                {
                    return await GetByModelAsync(model);
                }
                catch (Exception)
                {

                    throw new FileStorageException($"File not created {model.Name}");
                }
                /*var mappedParentFile = _mapper.Map<File, FileModel>(parentFile);
                mappedParentFile.ChildFileIds = new Collection<int>();
                mappedParentFile.ChildFileIds.Add(model.Id);
                await UpdateAsync(mappedParentFile);
                await _db.SaveAsync();*/

            }
            
        }

        public async Task<IEnumerable<FileModel>> GetFilesByParentIdAsync(int userId, int? parentId, string? sortType)
        {
            var userFiles = (await GetAllAsync())
                .Where(x => x.UserId == userId && x.ParentId == parentId);
            if (userFiles == null)
            {
                return null;
            }

            return SortFiles(userFiles, sortType);
        }

        public async Task<IEnumerable<FileModel>> GetFilesByUserIdAsync(int userId, string? sortType)
        {
            var userFiles = (await GetAllAsync())
                .Where(x => x.UserId == userId && x.ParentId == null);
            if(userFiles == null)
            {
                throw new FileStorageException("Files aren't exist");
            }

            return SortFiles(userFiles, sortType);
        }
        public async Task<IEnumerable<FileModel>> GetFilesByName(int userId, string fileName)
        {
            var userFiles = (await GetAllAsync())
                .Where(x => x.UserId == userId && x.Name.ToLower().Contains(fileName.ToLower()));
            if (userFiles == null)
            {
                throw new FileStorageException("Files aren't exist");
            }

            return userFiles;
        }

        public async Task<FileModel> UploadFileAsync(int userId, string? parentId, IFormFile formFile)
        {
            FileModel parent = null;
            int? resParentId = null;
            if (parentId != null)
            {
                resParentId = int.Parse(parentId);
                parent = await GetByIdAsync((int)resParentId);
            }
            var user = await _userService
                .GetByIdWithNoTrackingAsync(userId);
            if (user == null)
                throw new FileStorageException("No user with such an Id");
            //Here

            var userSpace = long.Parse(user.UsedDiskSpace ?? throw new FileStorageException("Null disk space"));

            if (userSpace + formFile.Length > long.Parse(user.AvailableDiskSpace))
                throw new FileStorageException("No space on Disk");

            user.UsedDiskSpace = (userSpace + formFile.Length).ToString();
            
            string path;

            //Here

            if(parent != null)
            {
                path = $"{_configuration["FilePath"]}\\{user.Id}\\{parent.Path}\\{formFile.FileName}";
            }
            else
            {
                path = $"{_configuration["FilePath"]}\\{user.Id}\\{formFile.FileName}";
            }

            if (System.IO.File.Exists(path)){
                throw new FileStorageException($"File already exist {formFile.FileName}");
            }

            using (var fileStream = new System.IO.FileStream(path, System.IO.FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            var fileModel = new FileModel
            {
                Name = formFile.FileName.Split(".")[0],
                Type = formFile.FileName.Split('.')[^1],
                Size = formFile.Length.ToString(),
                AccessLink = MD5Hash.GetMD5Hash(formFile.FileName.Split(".")[0] + formFile.FileName.Split('.')[^1]+ ";" + resParentId),
                Path = parent != null ? parent.Path + "\\" + formFile.FileName : formFile.FileName,
                ParentId = resParentId,
                UserId = userId
            };

            await AddAsync(fileModel);
            await _userService.UpdateAsync(user);

            try
            {
                return await GetByModelAsync(fileModel);
            }
            catch (Exception)
            {

                throw new FileStorageException($"File not created {fileModel.Name}");
            }

        }

        public async Task<DownloadFileModel> DownloadFileAsync(int userId, int fileId)
        {
            var file = await GetByIdAsync(fileId);
            if(file.UserId != userId)
            {
                throw new FileStorageException("This user doesn't have such a file");
            }
            
            var ext = file.Type.ToLower();
            return new DownloadFileModel(ext, System.IO.File.ReadAllBytes(_configuration["FilePath"] +  $"\\{userId}\\" + file.Path), file.Name);
        }
        public async Task<DownloadFileModel> DownloadFileByAccessLinkAsync(string accessLink)
        {
            var file = (await GetAllAsync()).FirstOrDefault(al => al.AccessLink == accessLink);
            if (file == null)
            {
                throw new FileStorageException("File isn't exist");
            }

            var ext = file.Type.ToLower();
            return new DownloadFileModel(ext, System.IO.File.ReadAllBytes(_configuration["FilePath"] + $"\\{file.UserId}\\" + file.Path), file.Name);
        }
        public void CreateDirWithAllInfo(FileModel file)
        {
            var filePath = $"{_configuration["FilePath"]}\\{file.UserId}\\{file.Path}";
            if (!System.IO.Directory.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            else
            {
                throw new FileStorageException("File already exists");
            }
        }

        private string GetPath(int userId, string filePath)
        {
            return _configuration["FilePath"] + $"\\{userId}\\" + filePath;
        }

        private bool DeleteLocalFiles(string path, string type)
        {
            if(type == "dir" && System.IO.Directory.Exists(path))
            {
                System.IO.Directory.Delete(path);
                return true;
            }
            if(type != "dir" && System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                return true;
            }
            else
            {
                return false;
            }
        }

        private IEnumerable<FileModel> SortFiles(IEnumerable<FileModel> files, string? sortType)
        {
            switch (sortType)
            {
                case "name":
                    return files.OrderBy(f => f.Name);
                case "type":
                    return files.OrderBy(f => f.Type);
                case "date":
                    return files.OrderBy(f => f.Date);
                default:
                    return files;
            }
        }
    }
}