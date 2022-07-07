using Business.Interfaces;
using Business.Models;
using Business.Services;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Data.Tests;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Business.Tests
{
    public class AdminServiceTests
    {
        private readonly FileStorageDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
        public readonly Mock<IUserService> _userServiceMock = new Mock<IUserService>();
        public AdminServiceTests()
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
            var expected = GetTestCustomerModels().Result.ToList();
            _userServiceMock.Setup(x => x.GetAllAsync())
                .Returns(GetTestCustomerModels());
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new AdminService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllUsersAsync(null, null)).ToList();

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetAllAsync_WithSortingTypeName_ShouldReturnAllSortedUserModels()
        {
            //Arrange
            var expectedSorting = "name";
            var expected = GetTestCustomerModels().Result.OrderBy(x => x.Email).ToList();
            _userServiceMock.Setup(x => x.GetAllAsync())
                .Returns(GetTestCustomerModels());
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new AdminService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllUsersAsync(expectedSorting, null)).ToList();

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetAllAsync_WithSortingTypeRole_ShouldReturnAllSortedUserModels()
        {
            //Arrange
            var expectedSorting = "role";
            var expected = GetTestCustomerModels().Result.OrderBy(x => x.RoleName).ToList();
            _userServiceMock.Setup(x => x.GetAllAsync())
                .Returns(GetTestCustomerModels());
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new AdminService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllUsersAsync(expectedSorting, null)).ToList();

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetAllAsync_WithNotExistingSortingType_ShouldReturnAllUserModelsWithoutAnySorting()
        {
            //Arrange
            var expectedSorting = "fgsdgfd";
            var expected = GetTestCustomerModels().Result.ToList();
            _userServiceMock.Setup(x => x.GetAllAsync())
                .Returns(GetTestCustomerModels());
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new AdminService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllUsersAsync(expectedSorting, null)).ToList();

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetAllAsync_WithExistingSortingTypeAndSearchingByEmail_ShouldReturnUserModelsWithThisEmail()
        {
            //Arrange
            var expectedEmail = "email1";
            var expectedSorting = "name";
            var expected = GetTestCustomerModels().Result.Where(x => x.Email.Contains(expectedEmail)).OrderBy(x => x.Email).ToList();
            _userServiceMock.Setup(x => x.GetAllAsync())
                .Returns(GetTestCustomerModels());
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new AdminService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllUsersAsync(expectedSorting, expectedEmail)).ToList();

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetAllAsync_WithExistingSortingTypeAndSearchingByEmail_ShouldReturnAllUserModelsWithThisEmail()
        {
            //Arrange
            var expectedEmail = "email";
            var expectedSorting = "name";
            var expected = GetTestCustomerModels().Result.Where(x => x.Email.Contains(expectedEmail)).OrderBy(x => x.Email).ToList();
            _userServiceMock.Setup(x => x.GetAllAsync())
                .Returns(GetTestCustomerModels());
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new AdminService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllUsersAsync(expectedSorting, expectedEmail)).ToList();

            //assert
            Assert.True(new UserModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetAllAsync_WithExistingSortingTypeAndSearchingByNotExistingEmail_ShouldReturnAllUserModelsWithThisEmail()
        {
            //Arrange
            var expectedEmail = "fdsfsfsd";
            var expectedSorting = "name";
            var expected = GetTestCustomerModels().Result.Where(x => x.Email.Contains(expectedEmail)).OrderBy(x => x.Email).ToList();
            _userServiceMock.Setup(x => x.GetAllAsync())
                .Returns(GetTestCustomerModels());
            _dbMock.Setup(x => x.UserRepository.GetAllWithDetailsAsync())
                .Returns(ReturnUsers);
            var userService = new AdminService(_dbMock.Object, SeedDb.CreateMapperProfile());

            //act
            var actual = (await userService.GetAllUsersAsync(expectedSorting, expectedEmail)).ToList();

            //assert
            Assert.Equal(expected, actual);

        }


        #region HelpData
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
                new User
                {
                    Id = 1,
                    Email = "email",
                    Password = "5f4dcc3b5aa765d61d8327deb882cf99",
                    UsedDiskSpade = "0",
                    Files = new Collection<File>(){
                    new File {Id = 1, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 2, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 3, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 4, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    new File {Id = 5, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                    }
                };
        }
        private async Task<IEnumerable<UserModel>> GetTestCustomerModels() { 
            return new List<UserModel>()
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
        #endregion
    }
}
