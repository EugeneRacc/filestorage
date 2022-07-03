using AutoMapper;
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

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync(string? sortType)
        {
            var userFiles = await _userService.GetAllAsync();
            if (userFiles == null)
            {
                return null;
            }
            return SortUsers(userFiles, sortType);
        }

        private IEnumerable<UserModel> SortUsers(IEnumerable<UserModel> users, string? sortType)
        {
            switch (sortType)
            {
                case "name":
                    return users.OrderBy(f => f.Email);
                case "role":
                    return users.OrderBy(f => f.RoleName);
                default:
                    return users;
            }
        }
    }
}
