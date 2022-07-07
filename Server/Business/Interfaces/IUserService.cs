using Business.Models;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    /// <summary>
    /// Interface with an extension of functionality for working with Users
    /// </summary>
    /// <seealso cref="Business.Interfaces.ICrud&lt;Business.Models.UserModel&gt;" />
    public interface IUserService : ICrud<UserModel>
    {
        /// <summary>
        /// Check log in data for loggin into client asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>True if data correct, False if data incorrect</returns>
        public Task<bool> LogInAsync(UserModel model);
        /// <summary>
        /// Gets the User by user credentials.
        /// </summary>
        /// <param name="login">The login.</param>
        /// <returns>UserModel</returns>
        public Task<UserModel> GetByUserCredentials(UserLogin login);
        /// <summary>
        /// Gets the User by identifier with no tracking asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>UserModel</returns>
        public Task<UserModel> GetByIdWithNoTrackingAsync(int id);
    }
}