using Business.Models;
using System.Threading.Tasks;

namespace Business.Interfaces
{

    public interface IUserService : ICrud<UserModel>
    {
        public Task<bool> LogInAsync(UserModel model);
    }
}