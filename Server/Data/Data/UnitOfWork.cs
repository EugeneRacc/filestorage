using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FileStorageDbContext _fileStorageDbContext;
        private IDiskSpaceRepository _diskSpaceRepository;
        private IFileRepository _fileRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
            _roleRepository = new RoleRepository(_fileStorageDbContext);

        }

        public IDiskSpaceRepository DiskSpaceRepository
        {
            get
            {
                if (_diskSpaceRepository == null)
                    _diskSpaceRepository = new DiskSpaceRepository(_fileStorageDbContext);
                return _diskSpaceRepository;
            }
        }

        public IFileRepository FileRepository
        {
            get
            {
                if (_fileRepository == null)
                    _fileRepository = new FileRepository(_fileStorageDbContext);
                return _fileRepository;
            }
        }

        public IRoleRepository RoleRepository
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_fileStorageDbContext);
                return _roleRepository;
            }
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_fileStorageDbContext);
                return _userRepository;
            }
        }
        public async Task SaveAsync()
        {
            
            await _fileStorageDbContext.SaveChangesAsync();
        }

        public async Task SaveAsync(User user)
        {
            await _fileStorageDbContext.SaveChangesAsync();
        }
    }
}