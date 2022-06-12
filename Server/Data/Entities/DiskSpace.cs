using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    //[Index(nameof(UserId), IsUnique = true)]
    public class DiskSpace : BaseEntity
    {
        public string AvailableDiskSpace { get; set; }
        public ICollection<User> Users { get; set; }
    }
}