using Data.Enum;

namespace Data.Entities
{
    
    public class Role : BaseEntity
    {
        public RoleType Name { get; set; }
        public User User { get; set; }
    }
}