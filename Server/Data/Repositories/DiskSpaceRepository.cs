using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class DiskSpaceRepository : IDiskSpaceRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;

        public DiskSpaceRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }
        public Task<IEnumerable<DiskSpace>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<DiskSpace> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(DiskSpace entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(DiskSpace entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(DiskSpace entity)
        {
            throw new System.NotImplementedException();
        }
    }
}