using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    /// <summary>
    /// Class with all business logic of Admin
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        public AdminService(IUnitOfWork uow, IMapper mapper, IConfiguration configuration)
        {
            _db = uow;
            _mapper = mapper;
            _configuration = configuration;
            _fileService = new FileService(uow, mapper, configuration);
            _userService = new UserService(uow, mapper, configuration);
        }
        public AdminService(IUnitOfWork uow, IMapper mapper)
        {
            _db = uow;
            _mapper = mapper;
            _userService = new UserService(uow, mapper);
            _fileService = new FileService(uow, mapper);
        }
        public async Task<IEnumerable<UserModel>> GetAllUsersAsync(string? sortType, string? searchingUser)
        {
            var users = await _userService.GetAllAsync();
            if (users == null)
            {
                throw new FileStorageException("User not found");
            }
            return SortUsers(users, sortType, searchingUser);
        }
        public async Task<UserModel> GetByIdAsync(int id)
        {
            return await _userService.GetByIdAsync(id);
        }

        public async Task<IEnumerable<FileModel>> GetUserFilesAsync(int userId, string? sortType, string? searchingName)
        {
            var userFiles = (await _fileService.GetAllAsync())
                .Where(x => x.UserId == userId);
            if (userFiles == null)
                throw new FileStorageException("Files not found");
            return SortFiles(userFiles, sortType, searchingName);
        }
        public async Task UpdateUserAsync(UserForUpdateModel model)
        {
            var userModel = await _userService.GetByIdWithNoTrackingAsync(model.Id);
            if (userModel == null)
                throw new FileStorageException("User was not found");
            Data.Entities.Role role;
            if (userModel.RoleId != null)
            {
                role = await _db.RoleRepository.GetByIdAsync((int)userModel.RoleId);
                role.RoleName = model.RoleName;
                _db.RoleRepository.Update(role);
            }
            userModel.RoleName = model.RoleName;
            userModel.Email = model.Email;
            await _userService.UpdateAsync(userModel);
        }
        private IEnumerable<UserModel> SortUsers(IEnumerable<UserModel> users, string? sortType, string? searchingUser)
        {
            if (searchingUser == null)
                searchingUser = "";
            switch (sortType)
            {
                case "name":
                    return users.Where(x => x.Email.Contains(searchingUser)).OrderBy(f => f.Email);
                case "role":
                    return users.Where(x => x.Email.Contains(searchingUser)).OrderBy(f => f.RoleName);
                default:
                    return users.Where(x => x.Email.Contains(searchingUser));
            }
        }
        private IEnumerable<FileModel> SortFiles(IEnumerable<FileModel> files, string? sortType, string? searchingName)
        {
            if (searchingName == null)
                searchingName = "";
            switch (sortType)
            {
                case "name":
                    return files.Where(x => x.Name.Contains(searchingName)).OrderBy(f => f.Name);
                case "type":
                    return files.Where(x => x.Name.Contains(searchingName)).OrderBy(f => f.Type);
                case "date":
                    return files.Where(x => x.Name.Contains(searchingName)).OrderBy(f => f.Date);
                default:
                    return files.Where(x => x.Name.Contains(searchingName));
            }
        }
       

    }
}
