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
            await _fileStorageDbContext.Set<User>().AddAsync(entity);
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
            else
            {
                throw new NullReferenceException($"Entity with such an id isn't exist {nameof(entity)}");
            }
        }

        public void Update(User entity)
        {
            // 
            var local = _fileStorageDbContext.Set<User>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entity.Id));

            // check if local is not null 
            if (local != null)
            {
                // detach
                _fileStorageDbContext.Entry(local).State = EntityState.Detached;
            }
            // set Modified flag in your entry
            _fileStorageDbContext.Entry(entity).State = EntityState.Modified;

            EntityEntry entityEntry = _fileStorageDbContext.Entry<User>(entity);
            entityEntry.State = EntityState.Modified;
        }

        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            var result = await _fileStorageDbContext.Users
                .Include(f => f.Files)
                .Include(ds => ds.DiskSpace)
                .Include(r => r.Role)
                .ToListAsync();
            return result;
        }

        public async Task<User> GetByIdWithDetailsAsync(int id)
        {
            var result = await _fileStorageDbContext.Users
                .Include(f => f.Files)
                .Include(ds => ds.DiskSpace)
                .Include(r => r.Role)
                .FirstOrDefaultAsync(u => u.Id == id);
            return result;
        }
    }
}