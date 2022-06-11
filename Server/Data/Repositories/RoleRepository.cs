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
    public class RoleRepository : IRoleRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;

        public RoleRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }
        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            var roles = await _fileStorageDbContext.Set<Role>().ToListAsync();
            return roles;
        }

        public async Task<Role> GetByIdAsync(int id)
        {
            var role = await _fileStorageDbContext.Set<Role>().FindAsync(id);
            return role;
        }

        public async Task AddAsync(Role entity)
        {
            await _fileStorageDbContext.Set<Role>().AddAsync(entity);
        }

        public void Delete(Role entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<Role>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _fileStorageDbContext.Set<Role>().FindAsync(id);
            if (entity != null)
            {
                Delete(entity);
            }
            else
            {
                throw new NullReferenceException($"Entity with such an id isn't exist {nameof(entity)}");
            }
        }

        public void Update(Role entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<Role>(entity);
            entityEntry.State = EntityState.Modified;
        }
    }
}