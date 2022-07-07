using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Models;
using Business.PasswordHash;
using Business.Services;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Data.Tests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Business.Tests
{
    public class UserServiceTests
    {
        private readonly FileStorageDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
        public UserServiceTests()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString() // Use GUID so every test will use a different db
                );
           
            _context = new FileStorageDbContext(dbOptions.Options);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllUserModels()
        {
            //Arrange
            var expected = GetTestCustomerModels;
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers());
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllAsync()).ToList();

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnUserModelWithId1()
        {
            //Arrange
            var expected = GetTestCustomerModels[0];
            _dbMock.Setup(x => x.UserRepository.GetByIdWithDetailsAsync(1))
                .Returns(GetOneUser());
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = await userService.GetByIdAsync(1);

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetByIdAsync_WithNoSuchAUser_ThrowFileStorageException()
        {
            //Arrange
            UserModel expected = null;
            _dbMock.Setup(x => x.UserRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .Returns(() => null);
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.GetByIdAsync(100);

            //assert
            await Assert.ThrowsAsync<FileStorageException>(act);

        }

        [Fact]
        public async Task GetByIdByUserCredentialsAsync_WithExistingEmailAndPassword_ShouldReturnUserModel()
        {
            //Arrange
            var expected = GetTestCustomerModels[0];
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers());
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = await userService.GetByUserCredentials(new UserLogin { Email = "email", Password="password"});

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetByIdByUserCredentialsAsync_WithNotEmailAndPassword_ShouldThrowException()
        {
            //Arrange
            UserModel expected = null;
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(() => null);
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.GetByUserCredentials(new UserLogin { Email = "email", Password = "password" });
            
            //assert
            await Assert.ThrowsAsync<FileStorageException>(act);

        }

        [Fact]
        public async Task AddAsync_WithCorrectData_ShouldAddUser()
        {
            //Arrange
            var expected = new UserModel
            {
                Email = "fdsdfsf",
                Password = "dfsfdsfdsfds",
                RoleName = "User"
            };
            _dbMock.Setup(x => x.UserRepository.GetAllAsync())
               .Returns(ReturnUsers());
            _dbMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<User>()));
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            await userService.AddAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);

        }

        [Fact]
        public async Task AddAsync_WithExistingEmailData_ShouldThrowFileStorageException()
        {
            //Arrange
            var expected = new UserModel
            {
                Email = "email",
                Password = "dfsfdsfdsfds",
                RoleName = "User"
            };
            _dbMock.Setup(x => x.UserRepository.GetAllAsync())
               .Returns(ReturnUsers());
            _dbMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<User>()));
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.AddAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Never);
            await Assert.ThrowsAsync<FileStorageException>(act);
        }

        [Fact]
        public async Task AddAsync_WithSmallPassword_ShouldThrowFileStorageException()
        {
            //Arrange
            var expected = new UserModel
            {
                Email = "gfdgfdgfdg",
                Password = "dfsfds",
                RoleName = "User"
            };
            _dbMock.Setup(x => x.UserRepository.GetAllAsync())
               .Returns(ReturnUsers());
            _dbMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<User>()));
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.AddAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Never);
            await Assert.ThrowsAsync<FileStorageException>(act);
        }

        [Fact]
        public async Task AddAsync_WithNullModel_ShouldThrowFileStorageException()
        {
            //Arrange
            UserModel expected = null;
            _dbMock.Setup(x => x.UserRepository.GetAllAsync())
               .Returns(ReturnUsers());
            _dbMock.Setup(x => x.UserRepository.AddAsync(It.IsAny<User>()));
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.AddAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Never);
            await Assert.ThrowsAsync<FileStorageException>(act);
        }

        [Fact]
        public async Task UpdateAsync_WithNullModel_ShouldThrowFileStorageException()
        {
            //Arrange
            UserModel expected = null;
            _dbMock.Setup(x => x.UserRepository.Update(It.IsAny<User>()));
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.UpdateAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Never);
            await Assert.ThrowsAsync<FileStorageException>(act);
        }

        [Fact]
        public async Task UpdateAsync_WithCorrectUser_ShouldSaveAsyncOnce()
        {
            //Arrange
            UserModel expected = new UserModel
            {
                Email = "email",
                Password = "password",
                RoleName = "User"
            };
            _dbMock.Setup(x => x.UserRepository.Update(It.IsAny<User>()));
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
           await userService.UpdateAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(It.IsAny<User>()), Times.Once);
            
        }

        [Fact]
        public async Task LogInAsync_WithCorrectData_ShouldReturnTrue()
        {
            //Arrange
            UserModel expected = new UserModel
            {
                Email = "email",
                Password = "password",
                RoleName = "User"
            };
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = await userService.LogInAsync(expected);

            //assert
            Assert.True(actual);

        }

        [Fact]
        public async Task LogInAsync_WithBadPassword_ShouldThrowFileStorageException()
        {
            //Arrange
            UserModel expected = new UserModel
            {
                Email = "email",
                Password = "password123",
                RoleName = "User"
            };
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.LogInAsync(expected);

            //assert
            await Assert.ThrowsAsync<FileStorageException>(act);

        }

        [Fact]
        public async Task LogInAsync_WithNotExistingEmail_ShouldThrowFileStorageException()
        {
            //Arrange
            UserModel expected = new UserModel
            {
                Email = "email432432",
                Password = "password",
                RoleName = "User"
            };
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            Func<Task> act = async () => await userService.LogInAsync(expected);

            //assert
            await Assert.ThrowsAsync<FileStorageException>(act);

        }

        [Fact]
        public async Task DeleteAsync_WithCorrectId_ShouldSaveAsyncOnce()
        {
            //Arrange
            int expected = 1;
            _dbMock.Setup(x => x.UserRepository.DeleteByIdAsync(It.IsAny<int>()))
                .Returns(ReturnUsers);
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            await userService.DeleteAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WithInCorrectId_ShouldSaveAsyncOnce()
        {
            //Arrange
            int expected = 100;
            _dbMock.Setup(x => x.UserRepository.DeleteByIdAsync(It.IsAny<int>()))
                .Returns(ReturnUsers);
            var userService = new UserService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            await userService.DeleteAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
        }
        private async Task<IEnumerable<User>> ReturnUsers()
        {
            return new List<User>()
            {
                new User{Id = 1, Email = "email", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0",
                    Files = new Collection<File>(){
                    new File {Id = 1, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 2, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 3, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 4, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 5, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    }},
                new User{Id = 2, Email = "email2", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 3, Email = "email3", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 4, Email = "email4", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 5, Email = "email5", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 6, Email = "email6", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 7, Email = "email7", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 8, Email = "email8", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 9, Email = "email9", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
                new User{Id = 10, Email = "email10", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0"},
            };
        }
        private async Task<User> GetOneUser()
        {
            return
                new User{Id = 1, Email = "email", Password = "5f4dcc3b5aa765d61d8327deb882cf99", UsedDiskSpade = "0",
                    Files = new Collection<File>(){
                    new File {Id = 1, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 2, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 3, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 4, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 5, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    }};
        }
        private List<UserModel> GetTestCustomerModels =>
            new List<UserModel>()
            {
                new UserModel{Id = 1, Email = "email", Password = "password", UsedDiskSpace = "0",
                    FilesIds = new Collection<int>(){ 1, 2, 3, 4, 5}, RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 2, Email = "email2", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 3, Email = "email3", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 4, Email = "email4", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 5, Email = "email5", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 6, Email = "email6", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 7, Email = "email7", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 8, Email = "email8", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 9, Email = "email9", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
                new UserModel{Id = 10, Email = "email10", Password = "password", UsedDiskSpace = "0", RoleId = 0, RoleName = "User", DiskSpaceId = 0 },
            };

    }
}