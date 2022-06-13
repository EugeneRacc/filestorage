using Business.Models;

namespace Business.Interfaces;

public interface IUserService : ICrud<UserModel>
{
    public Task<bool> LogInAsync(UserModel model);
}