using Data.Entities;

namespace Data.Interfaces
{
    /// <summary>
    /// Expansion of functionality of IRepository for better working with Roles
    /// </summary>
    /// <seealso cref="Data.Interfaces.IRepository&lt;Data.Entities.Role&gt;" />
    public interface IRoleRepository : IRepository<Role>
    {
        
    }
}