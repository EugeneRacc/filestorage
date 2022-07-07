using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Microsoft.EntityFrameworkCore;

namespace Data.Entities
{
    /// <summary>
    /// Class for creating DB table by ef
    /// </summary>
    /// <seealso cref="Data.Entities.BaseEntity" />
    public class DiskSpace : BaseEntity
    {
        public string AvailableDiskSpace { get; set; }
        public ICollection<User> Users { get; set; }
    }
}