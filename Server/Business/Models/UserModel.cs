using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using Data.Entities;
using Data.Enum;

namespace Business.Models
{

    public class UserModel
    {
        //TODO add business logic to UserModel
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; } = 1;
        public string UsedDiskSpace { get; set; } = "0";
        public ICollection<int>? FilesIds { get; set; }
    }
}