using System.Threading.Tasks;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FileStorageDbContext _fileStorageDbContext;
        private IDiskSpaceRepository _diskSpaceRepository;
        private IFileMetaRepository _fileMetaRepository;
        private IFileRepository _fileRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;

        public UnitOfWork(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
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

        public IFileMetaRepository FileMetaRepository
        {
            get
            {
                if (_fileMetaRepository == null)
                    _fileMetaRepository = new FileMetaRepository(_fileStorageDbContext);
                return _fileMetaRepository;
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
    }
}