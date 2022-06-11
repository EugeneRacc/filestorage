using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class FileStorageDbContext : DbContext
    {
        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users);
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserDiskSpace)
                .WithOne(uds => uds.User);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Files)
                .WithOne(f => f.User);
            modelBuilder.Entity<File>()
                .HasOne(f => f.FileMeta)
                .WithOne(fm => fm.File);
        }
        public DbSet<File> Files { get; set; }
        public DbSet<FileMeta> FileMetas { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserDiskSpace> UserDiskSpaces { get; set; }
    }
}