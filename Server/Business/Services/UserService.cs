using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
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

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        var customers = await db.UserRepository.GetAllAsync();
        var mappedCustomers = mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(customers);
        return mappedCustomers;
    }

    public async Task<UserModel> GetByIdAsync(int id)
    {
        var customer = await db.UserRepository.GetByIdAsync(id);
        var mappedCustomer = mapper.Map<UserModel>(customer);
        return mappedCustomer;
    }

    public async Task AddAsync(UserModel model)
    {
        if (model == null)
        {
            throw new FileStorageException($"The customer cannot be null {nameof(model)}");
        }

        var check = (await db.UserRepository.GetAllAsync())
            .FirstOrDefault(x => x.Email == model.Email);
        if (check != null)
        {
            throw new FileStorageException($"The customer with such an email already exist {nameof(model)}");
        }
        var customer = mapper.Map<UserModel, User>(model);
        await db.UserRepository.AddAsync(customer);
        await db.SaveAsync();
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