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
    public class FileRepository : IFileRepository
    {
        private readonly FileStorageDbContext _fileStorageDbContext;

        public FileRepository(FileStorageDbContext fileStorageDbContext)
        {
            _fileStorageDbContext = fileStorageDbContext;
        }

        public async Task<IEnumerable<File>> GetAllAsync()
        {
            var files = await _fileStorageDbContext.Set<File>().ToListAsync();
            return files;
        }

        public async Task<File> GetByIdAsync(int id)
        {
            var file = await _fileStorageDbContext.Set<File>().FindAsync(id);
            return file;
        }
        /// <summary>
        /// Gets all with details asynchronous.
        /// </summary>
        /// <returns>File with details about User and Disk space</returns>
        public async Task<IEnumerable<File>> GetAllWithDetailsAsync()
        {
            var result = await _fileStorageDbContext.Files
                .Include(f => f.ChildFiles)
                .Include(f => f.FileFolder)
                .ToListAsync();
            return result;
        }
        public async Task AddAsync(File entity)
        {
            await _fileStorageDbContext.Set<File>().AddAsync(entity);
        }

        public void Delete(File entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<File>(entity);
            entityEntry.State = EntityState.Deleted;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _fileStorageDbContext.Set<File>().FindAsync(id);
            if (entity != null)
            {
                Delete(entity);
            }
        }

        public void Update(File entity)
        {
            EntityEntry entityEntry = _fileStorageDbContext.Entry<File>(entity);
            entityEntry.State = EntityState.Modified;
        }
        
    }
}