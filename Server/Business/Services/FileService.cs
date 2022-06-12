using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Business.PasswordHash;
using Data.Entities;
using Data.Interfaces;
using File = Data.Entities.File;

namespace Business.Services;

public class FileService : IFileService
{
    private readonly IUnitOfWork db;
    private readonly IMapper mapper;
    public FileService(IUnitOfWork uow, IMapper mapper)
    {
        db = uow;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<FileModel>> GetAllAsync()
    {
        var files = await db.FileRepository.GetAllAsync();
        var mappedfiles = mapper.Map<IEnumerable<Data.Entities.File>, IEnumerable<FileModel>>(files);
        return mappedfiles;
    }

    public async Task<FileModel> GetByIdAsync(int id)
    {
        var file = await db.FileRepository.GetByIdAsync(id);
        var mappedfile = mapper.Map<Data.Entities.File, FileModel>(file);
        return mappedfile;
    }

    public async Task AddAsync(FileModel model)
    {
        if (model == null)
        {
            throw new FileStorageException($"The customer cannot be null {nameof(model)}");
        }
        var file = mapper.Map<FileModel, File>(model);
        await db.FileRepository.AddAsync(file);
        await db.SaveAsync();
    }

    public async Task UpdateAsync(FileModel model)
    {
        if (model == null)
        {
            throw new FileStorageException($"The user cannot be null {nameof(model)}");
        }
        db.FileRepository.Update(mapper.Map<File>(model));
        await db.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await db.FileRepository.DeleteByIdAsync(modelId);
        await db.SaveAsync();
        
    }
}