using System;
using Data.Enum;

namespace Data.Entities
{
    
    public class FileMeta : BaseEntity
    {
        public DateTime Creation { get; set; }
        public DateTime Modify { get; set; }
        public int FileId { get; set; }
        public ModificationType ModificationType { get; set; }
        public File File { get; set; }
    }
}