using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

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

    public Task<IEnumerable<FileModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<FileModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(FileModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(FileModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int modelId)
    {
        throw new NotImplementedException();
    }
}