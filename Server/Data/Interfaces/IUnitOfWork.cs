using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUnitOfWork
    {
        IDiskSpaceRepository DiskSpaceRepository { get; }
        IFileRepository FileRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        Task SaveAsync();
    }
}