using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace Data.Entities
{
    /// <summary>
    /// Class for creating File table in Db
    /// </summary>
    /// <seealso cref="Data.Entities.BaseEntity" />
    public class File : BaseEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string? AccessLink { get; set; }
        public string Size { get; set; }
        public string Path { get; set; }
        public int UserId { get; set; }
        public int? ParentId { get; set; }
        public DateTime Date { get; set; }
        public User User { get; set; }
        public File FileFolder { get; set; }
        public ICollection<File> ChildFiles { get; set; }

    }
}