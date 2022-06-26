namespace Business.Models
{

    public class FileModel
    {
        //TODO add business logic to FileModel
        public string Name { get; set; }
        public string Type { get; set; }
        public int? ParentId { get; set; }
        public int? UserId { get; set; }
        public string? Path { get; set; }
    }
}