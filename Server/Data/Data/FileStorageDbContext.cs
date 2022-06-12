using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Data
{
    public class FileStorageDbContext : DbContext
    {
        public FileStorageDbContext(DbContextOptions<FileStorageDbContext> options) : base(options) { }

        /*public FileStorageDbContext()
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=localhost,1433;Database=ms-sql-server;User=sa;Password=Docker@123;");
        }
*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users);
            modelBuilder.Entity<User>()
                .HasOne(u => u.DiskSpace)
                .WithMany(uds => uds.Users);
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
        public DbSet<DiskSpace> UserDiskSpaces { get; set; }
    }
}