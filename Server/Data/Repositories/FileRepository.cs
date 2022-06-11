using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories
{
    public class FileRepository : IFileMetaRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;

        public FileRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }
        public Task<IEnumerable<FileMeta>> GetAllAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<FileMeta> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public Task AddAsync(FileMeta entity)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(FileMeta entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Update(FileMeta entity)
        {
            throw new System.NotImplementedException();
        }
    }
}