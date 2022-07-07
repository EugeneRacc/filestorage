using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities;

namespace Data.Interfaces
{
    /// <summary>
    /// Expansion of functionality of IRepository for better working with Users
    /// </summary>
    /// <seealso cref="Data.Interfaces.IRepository&lt;Data.Entities.User&gt;" />
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> GetAllWithDetailsAsync();
        Task<User> GetByIdWithDetailsAsync(int id);
        public Task<User> GetByIdWithNoTrackAsync(int id);
    }
}