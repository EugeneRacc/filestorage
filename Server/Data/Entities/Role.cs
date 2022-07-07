using System.Collections.Generic;
using Data.Enum;

namespace Data.Entities
{
    /// <summary>
    /// Class for creating table with User Roles
    /// </summary>
    /// <seealso cref="Data.Entities.BaseEntity" />
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}