using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Business.PasswordHash;
using Data.Entities;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Services
{

    public class UserService : IUserService
    {
        private readonly IUnitOfWork db;
        private readonly IMapper mapper;
        private readonly IConfiguration configuration;
        public UserService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration)
        {
            db = uow;
            this.mapper = mapper;
            this.configuration = configuration;
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

        public async Task<UserModel> GetByUserCredentials(UserLogin userLogin)
        {
            userLogin.Password = MD5Hash.GetMD5Hash(userLogin.Password);
            var user = (await db.UserRepository.GetAllWithDetailsAsync()).FirstOrDefault(u =>
            u.Email.ToLower() == userLogin.Email.ToLower() && u.Password == userLogin.Password);
            var mappedUser = mapper.Map<UserModel>(user);
            return mappedUser;
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
            int id = customer.Id;
            if (customer == null)
                return;
            await db.UserRepository.AddAsync(customer);
            await db.SaveAsync();
            new FileService(db, mapper, configuration).CreateDirWithAllInfo(
                new FileModel { UserId = id, Name = $"{id}" });
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

        public async Task<bool> LogInAsync(UserModel model)
        {
            var user = (await GetAllAsync()).FirstOrDefault(x => x.Email == model.Email);
            if (user == null)
            {
                throw new NullReferenceException($"User with such email not found {model.Email}");
            }

            bool isPasswordValid = user.Password.Equals(MD5Hash.GetMD5Hash(model.Password));
            if (!isPasswordValid)
            {
                throw new FileStorageException($"Email or Password isn't valid");
            }

            return isPasswordValid;
        }
        public async Task DeleteAsync(int modelId)
        {
            await db.UserRepository.DeleteByIdAsync(modelId);
            await db.SaveAsync();
        }
    }
}