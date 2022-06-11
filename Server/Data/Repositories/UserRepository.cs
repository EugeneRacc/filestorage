using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;

        public UserRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(User entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(User entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(User entity)
        {
            throw new System.NotImplementedException();
        }
    }
}