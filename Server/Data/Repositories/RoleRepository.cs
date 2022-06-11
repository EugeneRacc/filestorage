using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;

        public RoleRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }
        public Task<IEnumerable<Role>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<Role> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(Role entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(Role entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(Role entity)
        {
            throw new System.NotImplementedException();
        }
    }
}