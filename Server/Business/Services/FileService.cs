using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Business.PasswordHash;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Services
{

    public class FileService : IFileService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public FileService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration)
        {
            _db = uow;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<IEnumerable<FileModel>> GetAllAsync()
        {
            var files = await _db.FileRepository.GetAllAsync();
            var mappedfiles = _mapper.Map<IEnumerable<Data.Entities.File>, IEnumerable<FileModel>>(files);
            return mappedfiles;
        }

        public async Task<FileModel> GetByIdAsync(int id)
        {
            var file = await _db.FileRepository.GetByIdAsync(id);
            var mappedfile = _mapper.Map<Data.Entities.File, FileModel>(file);
            return mappedfile;
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

        public async Task DeleteAsync(int modelId)
        {
            await _db.FileRepository.DeleteByIdAsync(modelId);
            await _db.SaveAsync();

        }

        public async Task CreateDir(FileModel model)
        {
            File parentFile = null;
            if (model.ParentId != null) {
                parentFile = await _db.FileRepository.GetByIdAsync((int)model.ParentId);
            }
            if(parentFile == null)
            {
                model.Path = model.Name;
                await AddAsync(model);
                CreateDirWithAllInfo(model);
            }
            else
            {
                model.Path = @$"{parentFile.Path}\\{model.Name}";
                CreateDirWithAllInfo(model);
                var mappedParentFile = _mapper.Map<File, FileModel>(parentFile);
                var mappedFile = _mapper.Map<FileModel, File>(model);
                parentFile.ChildFiles.Add(mappedFile);
                await UpdateAsync(mappedParentFile);
                await AddAsync(model);
                await _db.SaveAsync();
                
            }
            
        }

        public void CreateDirWithAllInfo(FileModel file)
        {
            var filePath = $"{_configuration["FilePath"]}\\\\{file.UserId}\\\\{file.Path}";
            if (!System.IO.File.Exists(filePath))
            {
                System.IO.Directory.CreateDirectory(filePath);
            }
            else
            {
                throw new FileStorageException("File already exists");
            }
        }

        
    }
}