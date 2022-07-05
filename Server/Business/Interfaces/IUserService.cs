using Business.Models;
using System.Threading.Tasks;

namespace Business.Interfaces
{

    public interface IUserService : ICrud<UserModel>
    {
        public Task<bool> LogInAsync(UserModel model);
        public Task<UserModel> GetByUserCredentials(UserLogin login);
        public Task<UserModel> GetByIdWithNoTrackingAsync(int id);
    }
}