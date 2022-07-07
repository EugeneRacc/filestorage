using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Repositories
{
    /// <summary>
    /// Class with CRUD methods for working with DiskSpace
    /// </summary>
    /// <seealso cref="Data.Interfaces.IDiskSpaceRepository" />
    public class DiskSpaceRepository : IDiskSpaceRepository
    {
        /// <summary>
        /// The file storage database context
        /// </summary>
        private readonly FileStorageDbContext _fileStorageDbContext;
        /// <summary>
        /// Initializes a new instance of the <see cref="DiskSpaceRepository"/> class.
        /// </summary>
        /// <param name="fileStorageDbContext">The file storage database context.</param>
        public DiskSpaceRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }
        public async Task<IEnumerable<DiskSpace>> GetAllAsync()
        {
            var diskSpaces = await _fileStorageDbContext.Set<DiskSpace>().ToListAsync();
            return diskSpaces;
        }
        public async Task<DiskSpace> GetByIdAsync(int id)
        {
            var diskSpace = await _fileStorageDbContext.Set<DiskSpace>().FindAsync(id);
            return diskSpace;
        }

        public async Task AddAsync(DiskSpace entity)
        {
            await _fileStorageDbContext.Set<DiskSpace>().AddAsync(entity);
        }

        public void Delete(DiskSpace entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<DiskSpace>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _fileStorageDbContext.Set<DiskSpace>().FindAsync(id);
            if (entity == null)
            {
                throw new NullReferenceException();
            }
            Delete(entity);
        }

        public void Update(DiskSpace entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<DiskSpace>(entity);
            entityEntry.State = EntityState.Modified;
        }
    }
}