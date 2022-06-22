using System.Collections.Generic;
using Data.Enum;

namespace Data.Entities
{
    
    public class Role : BaseEntity
    {
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}