using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    /// <summary>
    /// Class with Admin Functionality
    /// </summary>
    public interface IAdminService
    {
        /// <summary>
        /// Gets all users asynchronous.
        /// </summary>
        /// <param name="sortType">Type of the sort.</param>
        /// <param name="searchingUser">The searching user.</param>
        /// <returns>IEnumerable of UserModels</returns>
        public Task<IEnumerable<UserModel>> GetAllUsersAsync(string? sortType, string? searchingUser);
        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return User with needed ID</returns>
        public Task<UserModel> GetByIdAsync(int id);
        /// <summary>
        /// Gets the user files asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="sortType">Type of the sort.</param>
        /// <param name="searchingName">Name of the searching.</param>
        /// <returns>Return all files that are owned by user</returns>
        public Task<IEnumerable<FileModel>> GetUserFilesAsync(int userId, string? sortType, string? searchingName);
        /// <summary>
        /// Updates the user asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>Task</returns>
        public Task UpdateUserAsync(UserForUpdateModel model);
    }
}
