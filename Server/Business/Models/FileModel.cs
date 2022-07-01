using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Business.Models
{

    public class FileModel
    {
        //TODO add business logic to FileModel
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public string? Size { get; set; }
        public string? AccessLink { get; set; }
        public int? ParentId { get; set; }
        public int? UserId { get; set; }
        public string? Path { get; set; }
        public ICollection<int>? ChildFileIds { get; set; }
    }
}