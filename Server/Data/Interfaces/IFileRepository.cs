using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface IFileRepository : IRepository<File>
    {
        Task<IEnumerable<File>> GetAllWithDetailsAsync();
        Task<File> GetByIdWithDetailsAsync(int id);
    }
}