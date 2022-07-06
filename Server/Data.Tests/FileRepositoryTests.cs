using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Data.Tests
{
    public class FileRepositoryTests
    {
        
            private readonly FileStorageDbContext _context;
            private readonly IFileRepository _repository;
            private readonly IUnitOfWork _unitOfWork;
            public FileRepositoryTests()
            {
                DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString() // Use GUID so every test will use a different db
                    );

                _context = new FileStorageDbContext(dbOptions.Options);
                _unitOfWork = new UnitOfWork(_context);
                _repository = new FileRepository(_context);
            }

            [Fact]
            public async Task AddFileAsync_ShouldAddFileToDb()
            {
            // Arrange
                var file = GetOneFileModel();
                

                // Act
                await CreateFile(file);
                var files = _context.Files.ToList();
                // Assert
                Assert.Single(files);
                Assert.Equal(file, files[0]);
            }

            [Fact]
            public async Task AddFileAsync_ShouldThrowException()
            {
                // Arrange
                File file = null;

                //Act
                Func<Task> act = () => CreateFile(file);

                // Assert & Act

                await Assert.ThrowsAsync<ArgumentNullException>(act);

            }

            [Fact]
            public async Task GetAllAsync_ShouldReturn4Files()
            {
                // Arrange
                Seed();
                int expectedLength = GetExpectedFiles().ToList().Count;
                var expected = GetExpectedFiles().ToList();
                //Act
                var resultUsers = (await _repository.GetAllAsync()).ToList();
                var actual = (await _repository.GetAllAsync()).ToList();
                // Assert

                Assert.Equal(expectedLength, resultUsers.Count);
                Assert.True(new FileEqualityComparer().Equals(expected, actual));
            }

            [Fact]
            public async Task GetAllAsync_ShouldReturnSameFiles()
            {
                // Arrange
                Seed();
                var expected = GetExpectedFiles().ToList();
                //Act
                var actual = (await _repository.GetAllAsync()).ToList();
                // Assert

                Assert.True(new FileEqualityComparer().Equals(expected, actual));

            }


            [Fact]
            public async Task GetByIdAsync_ShouldReturnOneFileWithId1()
            {
                // Arrange
                Seed();
                var expectedUser = GetExpectedFiles().FirstOrDefault(x => x.Id == 1);
                //Act
                var resultUser = await _repository.GetByIdAsync(1);

                // Assert
                Assert.True(new FileEqualityComparer().Equals(expectedUser, resultUser));
                Assert.Equal(expectedUser.Id, resultUser.Id);

            }

            [Fact]
            public async Task GetByIdAsync_WithNotExistingFile_ShouldReturnNull()
            {
                // Arrange
                Seed();
                var expectedUser = GetExpectedFiles().FirstOrDefault(x => x.Id == 100);
                //Act
                var resultUser = await _repository.GetByIdAsync(100);

                // Assert
                Assert.True(new FileEqualityComparer().Equals(expectedUser, resultUser));
            }

            [Fact]
            public async Task Delete_ByUserId_ShouldDeleteUserFromDB()
            {
                // Arrange
                Seed();
                var expectedLength = GetExpectedFiles().ToList().Count - 1;

                //Act
                await DeleteFileById(GetOneFileModel().Id);
                var actualLength = _context.Files.ToList().Count;
                // Assert
                Assert.Equal(expectedLength, actualLength);
            }

            [Fact]
            public async Task DeleteByIdAsync_ByNotExistingId_ShouldPassAndNotDeleteUser()
            {
                // Arrange
                Seed();
                var expectedLength = GetExpectedFiles().ToList().Count;

                //Act
                await DeleteFileById(100);
                var actualLength = _context.Files.ToList().Count;
                // Assert
                Assert.Equal(expectedLength, actualLength);
            }

            [Fact]
            public async Task Delete_ByUserModel_ShouldDeleteUserFromDB()
            {
                // Arrange
                Seed();
                var expectedLength = GetExpectedFiles().ToList().Count - 1;

                //Act
                await DeleteFile(GetOneFileModel());
                var actualLength = _context.Users.ToList().Count;
                // Assert
                Assert.Equal(expectedLength, actualLength);
            }

            [Fact]
            public async Task GetAllAsyncWithDetails_ShouldReturnTenUsers()
            {
                // Arrange
                Seed();
                var expected = GetExpectedFiles().ToList();
                //Act
                var actual = (await _repository.GetAllWithDetailsAsync()).ToList();

                // Assert

                Assert.True(new FileEqualityComparer().Equals(expected[0], actual[0]));

            }

            private void Seed()
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

            private IEnumerable<File> GetExpectedFiles()
            {
                var files = new List<File>()
                {
                    new File {Id = 1, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 2, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 3, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 4, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 5, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                };

            return files;
            }
            private File GetOneFileModel()
            {
                return
                   new File
                   {
                       Id = 1,
                       Name = "1",
                       Type = "dir",
                       AccessLink = "we",
                       Size = "0",
                       UserId = 1,
                       ParentId = null,
                       Path = "",
                       Date = DateTime.Now
                   };
        }
            private async Task CreateFile(File entity)
            {
                await _repository.AddAsync(entity);
                await _context.SaveChangesAsync();
            }
            private async Task DeleteFile(File entity)
            {
                _repository.Delete(entity);
                await _context.SaveChangesAsync();
            }
            private async Task DeleteFileById(int id)
            {
                await _repository.DeleteByIdAsync(id);
                await _context.SaveChangesAsync();
            }
        }
    }

