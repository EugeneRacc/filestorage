using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [Index(nameof(Email), IsUnique=true)]
    public class User : BaseEntity
    {
        [Required(ErrorMessage = "This field is required.")]
        [MaxLength(200, ErrorMessage = "Email cannot be more than 200 characters.")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; }

        public int RoleId { get; set; }
        public int UserDiskSpaceId { get; set; }
        public Role Role { get; set; }
        public UserDiskSpace UserDiskSpace { get; set; }
        public ICollection<File> Files { get; set; }
    }
}