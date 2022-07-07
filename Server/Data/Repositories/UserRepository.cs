using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;
        public UserRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = await _fileStorageDbContext.Set<User>().ToListAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _fileStorageDbContext.Set<User>().FindAsync(id);
            return user;
        }

        public async Task AddAsync(User entity)
        {
            await _fileStorageDbContext.Users.AddAsync(entity);
        }

        public void Delete(User entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<User>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _fileStorageDbContext.Set<User>().FindAsync(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void Update(User entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<User>(entity);
            entityEntry.State = EntityState.Modified;
        }
        /// <summary>
        /// Gets all with details asynchronous.
        /// </summary>
        /// <returns>Return Users with detail info about other Entities</returns>
        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            var result = await _fileStorageDbContext.Users
                .Include(f => f.Files)
                .Include(ds => ds.DiskSpace)
                .Include(r => r.Role)
                .ToListAsync();
            return result;
        }
        /// <summary>
        /// Gets the by identifier with details asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return User with detail info about other Entities</returns>
        public async Task<User> GetByIdWithDetailsAsync(int id)
        {
            var result = await _fileStorageDbContext.Users
                .Include(f => f.Files)
                .Include(ds => ds.DiskSpace)
                .Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            return result;
        }
        /// <summary>
        /// Gets the by identifier with no track asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Return Users with no tracking in cache</returns>
        public async Task<User> GetByIdWithNoTrackAsync(int id)
        {
            var result = await _fileStorageDbContext.Users.AsNoTracking()
                .Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            return result;
        }
    }
}