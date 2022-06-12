using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    [Index(nameof(UserId), IsUnique = true)]
    public class DiskSpace : BaseEntity
    {
        public string AvailableDiskSpace { get; set; }
        public string UsedDiskSpace { get; set; } = "0";
        public int UserId { get; set; }
        public User User { get; set; }
    }
}