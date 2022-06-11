using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data.Repositories
{
    public class FileMetaRepository : IFileMetaRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;

        public FileMetaRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }
        public async Task<IEnumerable<FileMeta>> GetAllAsync()
        {
            var filesMeta = await _fileStorageDbContext.Set<FileMeta>().ToListAsync();
            return filesMeta;
        }

        public async Task<FileMeta> GetByIdAsync(int id)
        {
            var fileMeta = await _fileStorageDbContext.Set<FileMeta>().FindAsync(id);
            return fileMeta;
        }

        public async Task AddAsync(FileMeta entity)
        {
            await _fileStorageDbContext.Set<FileMeta>().AddAsync(entity);
        }

        public void Delete(FileMeta entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<FileMeta>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _fileStorageDbContext.Set<FileMeta>().FindAsync(id);
            if (entity == null)
            {
                throw new NullReferenceException();
            }
            Delete(entity);
        }

        public void Update(FileMeta entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<FileMeta>(entity);
            entityEntry.State = EntityState.Modified;
        }
    }
}