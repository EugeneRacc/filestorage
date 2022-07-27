using AutoMapper;
using Business;
using Data.Data;
using Data.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Tests
{
    public class SeedDb
    {
        FileStorageDbContext _context;
        public SeedDb(FileStorageDbContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            var users = new List<User>()
            {
                new User{Id = 1, Email = "email", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 2, Email = "email2", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 3, Email = "email3", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 4, Email = "email4", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 5, Email = "email5", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 6, Email = "email6", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 7, Email = "email7", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 8, Email = "email8", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 9, Email = "email9", Password = "password", UsedDiskSpade = "0"},
                new User{Id = 10, Email = "email10", Password = "password", UsedDiskSpade = "0"},
            };
            var roles = new List<Role>()
            {
                new Role {Id = 1, RoleName = "Admin"},
                new Role {Id = 2, RoleName = "User"},
            };
            var files = new List<File>()
            {
                new File {Id = 1, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new File {Id = 2, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new File {Id = 3, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new File {Id = 4, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new File {Id = 5, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
            };
            var diskSpace = new List<DiskSpace>()
            {
                new DiskSpace { Id = 1, AvailableDiskSpace = "9030904932"},
                new DiskSpace { Id = 2, AvailableDiskSpace = "4334"},
            };
            _context.Users.AddRange(users);
            _context.Roles.AddRange(roles);
            _context.Files.AddRange(files);
            _context.UserDiskSpaces.AddRange(diskSpace);
            _context.SaveChanges();
        }
        public static IMapper CreateMapperProfile()
        {
            var myProfile = new AutomapperProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));

            return new Mapper(configuration);
        }

        public static IConfiguration CreateConfiguration()
        {
            var myConfiguration = new Dictionary<string, string>
                {
                    {"AllowedHosts", "*"},
                    {"Jwt:Key", "DhftOS5uphK3vmCJQrexST1RsyjZBjXWRgJMFPU4"},
                    {"Jwt:Issuer", "https://localhost:44368/"},
                    {"Jwt:Audience", "https://localhost:44368/"},
                };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            return configuration;
        }

    }
}
