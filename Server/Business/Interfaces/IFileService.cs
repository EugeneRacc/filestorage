using Business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Interfaces
{

    public interface IFileService : ICrud<FileModel>
    {
        public Task CreateDir(FileModel model);
        public Task<IEnumerable<FileModel>> GetFilesByParentIdAsync(int userId, int? parentId);
        public Task<IEnumerable<FileModel>> GetFilesByUserIdAsync(int userId);
    }
}