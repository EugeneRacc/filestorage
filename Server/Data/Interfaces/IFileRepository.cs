using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    public interface IFileRepository : IRepository<File>
    {
        public Task<IEnumerable<File>> GetAllWithDetailsAsync();
    }
}