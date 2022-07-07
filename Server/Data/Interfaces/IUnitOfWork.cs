using Data.Data;
using Data.Entities;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    /// <summary>
    /// Interface for using DI for UnitOfWork
    /// </summary>
    public interface IUnitOfWork
    {
        IDiskSpaceRepository DiskSpaceRepository { get; }
        IFileRepository FileRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
        Task SaveAsync(User user);
    }
}