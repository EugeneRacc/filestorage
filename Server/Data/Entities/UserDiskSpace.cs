using System;
using System.Numerics;
using System.Threading;

namespace Data.Entities
{
    public class UserDiskSpace : BaseEntity
    {
        public BigInteger DiskSpace { get; set; } = (BigInteger)Math.Pow(1024, 3) * 5;
        public BigInteger UsedDiskSpace { get; set; } = 0;
        public User User { get; set; }
    }
}