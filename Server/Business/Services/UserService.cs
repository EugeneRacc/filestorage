using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Business.PasswordHash;
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
        var customers = await db.UserRepository.GetAllWithDetailsAsync();
        var mappedCustomers = mapper.Map<IEnumerable<User>, IEnumerable<UserModel>>(customers);
        return mappedCustomers;
    }

    public async Task<UserModel> GetByIdAsync(int id)
    {
        var customer = await db.UserRepository.GetByIdWithDetailsAsync(id);
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

        if (model.Password.Length < 8)
        {
            throw new FileStorageException($"Password can't be less then 8 characters {nameof(model.Password)}");
        }
        model.Password = MD5Hash.GetMD5Hash(model.Password);
        var customer = mapper.Map<UserModel, User>(model);
        if (customer == null)
            return;
        await db.UserRepository.AddAsync(customer);
        await db.SaveAsync();
    }

    public async Task UpdateAsync(UserModel model)
    {
        if (model == null)
        {
            throw new FileStorageException($"The user cannot be null {nameof(model)}");
        }
        if (model.Password.Length < 8)
        {
            throw new FileStorageException($"The user cannot have such an easy password {nameof(model)}");
        }

        db.UserRepository.Update(mapper.Map<User>(model));
        await db.SaveAsync();

    }

    public async Task DeleteAsync(int modelId)
    {
        await db.UserRepository.DeleteByIdAsync(modelId);
        await db.SaveAsync();
    }
}