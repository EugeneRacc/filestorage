using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Data
{
    /// <summary>
    /// Class for setting DB
    /// </summary>
    /// <seealso cref="Microsoft.EntityFrameworkCore.DbContext" />
    public class FileStorageDbContext : DbContext
    {
        public FileStorageDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users);
            modelBuilder.Entity<User>()
                .Property(u => u.UsedDiskSpade)
                .HasDefaultValue("0");
            modelBuilder.Entity<User>()
                .Property(u => u.RoleId)
                .HasDefaultValue(1);
            modelBuilder.Entity<User>()
                .Property(u => u.DiskSpaceId)
                .HasDefaultValue(1);
            modelBuilder.Entity<User>()
                .HasOne(u => u.DiskSpace)
                .WithMany(uds => uds.Users);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Files)
                .WithOne(f => f.User);
            modelBuilder.Entity<File>()
                .Property(u => u.Size)
                .HasDefaultValue("0");
            modelBuilder.Entity<File>()
                .Property(u => u.Date)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<File>()
                .Property(u => u.AccessLink)
                .HasDefaultValueSql("HASHBYTES('MD5', CAST(getdate() AS nvarchar(50)))");
            modelBuilder.Entity<File>()
                .HasMany(e => e.ChildFiles)
                .WithOne(x => x.FileFolder)
                .HasForeignKey(e => e.ParentId);
        }
        public DbSet<File> Files { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DiskSpace> UserDiskSpaces { get; set; }
    }
}