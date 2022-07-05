using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Data.Entities;
using Data.Enum;

namespace Business.Models
{

    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; }
        public string RoleName { get; set; } = "User";
        public string? UsedDiskSpace { get; set; }
        public int? DiskSpaceId { get; set; }
        public int? RoleId { get; set; }
        public ICollection<int>? FilesIds { get; set; }
    }
}