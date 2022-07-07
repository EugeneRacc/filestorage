using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    /// <summary>
    /// Expansion of functionality of IRepository for better working with Files
    /// </summary>
    /// <seealso cref="Data.Interfaces.IRepository&lt;Data.Entities.File&gt;" />
    public interface IFileRepository : IRepository<File>
    {
        public Task<IEnumerable<File>> GetAllWithDetailsAsync();
    }
}