using System.Linq;
using System.Threading.Tasks;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Data.Data
{
    /// <summary>
    /// Class for working with Repositories in easier way
    /// </summary>
    /// <seealso cref="Data.Interfaces.IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        /// <summary>
        /// The file storage database context
        /// </summary>
        private readonly FileStorageDbContext _fileStorageDbContext;
        /// <summary>
        /// The disk space repository
        /// </summary>
        private IDiskSpaceRepository _diskSpaceRepository;
        /// <summary>
        /// The file repository
        /// </summary>
        private IFileRepository _fileRepository;
        /// <summary>
        /// The role repository
        /// </summary>
        private IRoleRepository _roleRepository;
        /// <summary>
        /// The user repository/
        /// </summary>
        private IUserRepository _userRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="fileStorageDbContext">The file storage database context.</param>
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
        /// <summary>
        /// Saves the asynchronous.
        /// </summary>
        public async Task SaveAsync()
        {
            
            await _fileStorageDbContext.SaveChangesAsync();
        }
        /// <summary>
        /// Saves with User the asynchronous.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task SaveAsync(User user)
        {
            await _fileStorageDbContext.SaveChangesAsync();
        }
    }
}