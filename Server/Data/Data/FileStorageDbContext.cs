using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class FileStorageDbContext : DbContext
    {
        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Fluent Api
            
        }
        public DbSet<File> Files { get; set; }
        public DbSet<FileMeta> FileMetas { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDiskSpace> UserDiskSpaces { get; set; }
    }
}