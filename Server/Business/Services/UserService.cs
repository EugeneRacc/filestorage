using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork db;
    private readonly IMapper mapper;
    public UserService(IUnitOfWork uow, IMapper mapper)
    {
        db = uow;
        this.mapper = mapper;
    }

    public Task<IEnumerable<UserModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(UserModel model)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(UserModel model)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(int modelId)
    {
        throw new NotImplementedException();
    }
}