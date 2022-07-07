using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    /// <summary>
    /// Model for updating User with Properties that can be updated
    /// </summary>
    public class UserForUpdateModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; } = "User";
        public int? RoleId { get; set; }
    }
}
