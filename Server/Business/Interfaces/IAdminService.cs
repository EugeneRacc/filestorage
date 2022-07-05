using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAdminService
    {
        public Task<IEnumerable<UserModel>> GetAllUsersAsync(string? sortType, string? searchingUser);
        public Task<UserModel> GetByIdAsync(int id);
        public Task<IEnumerable<FileModel>> GetUserFilesAsync(int userId, string? sortType, string? searchingName);
        public Task UpdateUserAsync(UserForUpdateModel model);
    }
}
