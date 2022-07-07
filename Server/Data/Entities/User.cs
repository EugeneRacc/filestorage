using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    /// <summary>
    /// Class for creating User Table in DB
    /// </summary>
    /// <seealso cref="Data.Entities.BaseEntity" />
    [Index(nameof(Email), IsUnique=true)]
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(200, ErrorMessage = "Email cannot be more than 200 characters.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }

        public string UsedDiskSpade { get; set; }
        public int RoleId { get; set; }
        public int DiskSpaceId { get; set; }
        public Role Role { get; set; }
        public DiskSpace DiskSpace { get; set; }
        public ICollection<File> Files { get; set; }
    }
}