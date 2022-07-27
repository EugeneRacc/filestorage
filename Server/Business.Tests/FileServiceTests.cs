using Business.Exceptions;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Business.Tests
{
    public class FileServiceTests
    {
        private readonly FileStorageDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public readonly Mock<IUnitOfWork> _dbMock = new Mock<IUnitOfWork>();
        public FileServiceTests()
        {
            DbContextOptionsBuilder dbOptions = new DbContextOptionsBuilder()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString() // Use GUID so every test will use a different db
                );

            _context = new FileStorageDbContext(dbOptions.Options);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllFileModels()
        {
            //Arrange
            var expected = GetTestFileModels;
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetAllAsync()).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnFileModelWithId1()
        {
            //Arrange
            var expected = GetTestFileModels[0];
            _dbMock.Setup(x => x.FileRepository.GetByIdAsync(1))
                .Returns(GetOneFile());
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = await fileService.GetByIdAsync(1);

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));
            Assert.Equal(1, actual.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WithNoSuchAFile_ThrowFileStorageException()
        {
            //Arrange
            FileModel expected = null;
            _dbMock.Setup(x => x.FileRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(() => null);
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            Func<Task> act = async () => await fileService.GetByIdAsync(100);

            //assert
            await Assert.ThrowsAsync<FileStorageException>(act);

        }

        [Fact]
        public async Task GetBymodelAsync_WithExistingModel_ShouldReturnFileModel()
        {
            //Arrange
            var expected = GetTestFileModels[0];
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());
  
            //act
            var actual = await fileService.GetByModelAsync(expected);

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));
            Assert.Equal(expected.Id, actual.Id);
        }

        [Fact]
        public async Task GetByModelAsync_WithNotExistingModel_ShouldThrowFileStorageException()
        {
            //Arrange
            var expected = GetTestFileModels[0];
            expected.Name = "fdsfds";
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            Func<Task> act = async () => await fileService.GetByModelAsync(expected);

            //assert
            await Assert.ThrowsAsync<FileStorageException>(act);
        }

        [Fact]
        public async Task AddAsync_WithCorrectData_ShouldAddUser()
        {
            //Arrange
            var expected = GetTestFileModels[0];
            _dbMock.Setup(x => x.FileRepository.AddAsync(It.IsAny<File>()));
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            await fileService.AddAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);

        }

        [Fact]
        public async Task AddAsync_WithNullModel_ShouldThrowFileStorageException()
        {
            //Arrange
            FileModel expected = null;
            _dbMock.Setup(x => x.FileRepository.AddAsync(It.IsAny<File>()));
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            Func<Task> act = async () => await fileService.AddAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Never);
            await Assert.ThrowsAsync<FileStorageException>(act);
        }

        [Fact]
        public async Task UpdateAsync_WithNullModel_ShouldThrowFileStorageException()
        {
            //Arrange
            FileModel expected = null;
            _dbMock.Setup(x => x.FileRepository.Update(It.IsAny<File>()));
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            Func<Task> act = async () => await userService.UpdateAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Never);
            await Assert.ThrowsAsync<FileStorageException>(act);
        }

        [Fact]
        public async Task UpdateAsync_WithCorrectFile_ShouldSaveAsyncOnce()
        {
            //Arrange
            FileModel expected = GetTestFileModels[0];
            _dbMock.Setup(x => x.FileRepository.Update(It.IsAny<File>()));
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            await fileService.UpdateAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);

        }

        [Fact]
        public async Task DeleteAsync_WithCorrectModelIdAndUserId_ShouldThrowFileStorageException()
        {
            //Arrange
            int expected = 1;
            _dbMock.Setup(x => x.FileRepository.GetByIdAsync(It.IsAny<int>()))
                .Returns(GetOneFile());
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            Func<Task> act = async () => await fileService.DeleteAsync(expected, userId: 1);

            //assert
            await Assert.ThrowsAsync<FileStorageException>(act);
            _dbMock.Verify(x => x.SaveAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WithModelId_ShouldSaveAsyncOnce()
        {
            //Arrange
            int expected = 1;
            _dbMock.Setup(x => x.FileRepository.DeleteByIdAsync(It.IsAny<int>()))
                .Returns(GetOneFile());
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            await fileService.DeleteAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);
           
        }

        [Fact]
        public async Task DeleteAsync_WithIncorectModelId_ShouldSaveAsyncOnce()
        {
            //Arrange
            int expected = 100;
            _dbMock.Setup(x => x.FileRepository.DeleteByIdAsync(It.IsAny<int>()))
                .Returns(GetOneFile());
            var fileService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            await fileService.DeleteAsync(expected);

            //assert
            _dbMock.Verify(x => x.SaveAsync(), Times.Once);

        }

        [Fact]
        public async Task GetFilesByParentIdAsync_WithCorrectUserId_ShouldReturnAllUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByParentIdAsync(userId: expectedUserId, parentId: null, sortType: null)).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByParentIdAsync_WithInCorrectUserId_ShouldReturnAllUserFileModels()
        {
            //Arrange
            var expectedUserId = 100;
            List<FileModel> expected = new List<FileModel>();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByParentIdAsync(userId: expectedUserId, parentId: null, sortType: null)).ToList();

            //assert
            Assert.Equal(expected, actual);

        }

        [Fact]
        public async Task GetFilesByParentIdAsync_WithCorrectUserIdAndSortTypeByDate_ShouldReturnAllSortedUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).OrderBy(x => x.Date).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByParentIdAsync(userId: expectedUserId, parentId: null, sortType: "date")).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByParentIdAsync_WithCorrectUserIdAndSortTypeByName_ShouldReturnAllSortedUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).OrderBy(x => x.Name).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByParentIdAsync(userId: expectedUserId, parentId: null, sortType: "name")).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByParentIdAsync_WithCorrectUserIdAndSortTypeByType_ShouldReturnAllSortedUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).OrderBy(x => x.Type).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByParentIdAsync(userId: expectedUserId, parentId: null, sortType: "type")).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByParentIdAsync_WithCorrectUserIdAndWithNoSuchTypeOfSort_ShouldReturnAllSortedUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByParentIdAsync(userId: expectedUserId, parentId: null, sortType: "fdsfds")).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByParentIdAsync_WithCorrectUserIdAndWithSortAndPerentId_ShouldReturnAllSortedUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expectedParentId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId && x.ParentId == expectedParentId).OrderBy(y => y.Date).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByParentIdAsync(userId: expectedUserId, parentId: expectedParentId, sortType: "date")).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByUserIdAsync_WithCorrectUserId_ShouldReturnAllUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByUserIdAsync(userId: expectedUserId,sortType: null)).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByUserIdAsync_WithInCorrectUserId_ShouldReturnAllUserFileModels()
        {
            //Arrange
            var expectedUserId = 100;
            List<FileModel> expected = new List<FileModel>();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByUserIdAsync(userId: expectedUserId, sortType: null)).ToList();

            //assert
            Assert.Equal(expected, actual);

        }

        [Fact]
        public async Task GetFilesByUserIdAsync_WithCorrectUserIdAndSortTypeByDate_ShouldReturnAllSortedUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).OrderBy(x => x.Date).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByUserIdAsync(userId: expectedUserId, sortType: "date")).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByUserIdAsync_WithCorrectUserIdAndSortTypeByName_ShouldReturnAllSortedUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId).OrderBy(x => x.Name).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByUserIdAsync(userId: expectedUserId, sortType: "name")).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByName_WithCorrectUserIdAndExistingFileName_ShouldReturnAllUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expectedName = "1";
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId && x.Name.Contains(expectedName)).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByName(userId: expectedUserId, expectedName)).ToList();

            //assert
            Assert.True(new FileModelEqualityComparer().Equals(expected, actual));

        }

        [Fact]
        public async Task GetFilesByName_WithInCorrectUserIdAndWithoutExistingFileName_ShouldReturnAllUserFileModels()
        {
            //Arrange
            var expectedUserId = 1;
            var expectedName = "2";
            var expected = GetTestFileModels.Where(x => x.UserId == expectedUserId && x.Name.Contains(expectedName)).ToList();
            _dbMock.Setup(x => x.FileRepository.GetAllWithDetailsAsync())
                .Returns(ReturnFiles());
            var userService = new FileService(_dbMock.Object, SeedDb.CreateMapperProfile(), SeedDb.CreateConfiguration());

            //act
            var actual = (await userService.GetFilesByName(userId: expectedUserId, expectedName)).ToList();

            //assert
            Assert.Equal(expected, actual);

        }
        #region HelpData

        private async Task<IEnumerable<File>> ReturnFiles()
        {
            return new List<File>()
            {
                new File {Id = 1, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new File {Id = 2, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = 1, Path = "", Date = DateTime.Now},
                new File {Id = 3, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new File {Id = 4, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new File {Id = 5, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
            };
        }
        private List<FileModel> GetTestFileModels =>
           new List<FileModel>()
            {
                new FileModel {Id = 1, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new FileModel {Id = 2, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = 1, Path = "", Date = DateTime.Now},
                new FileModel {Id = 3, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new FileModel {Id = 4, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
                new FileModel {Id = 5, Name = "1", Type = "dir", AccessLink="we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now},
            };

        private async Task<File> GetOneFile()
        {
            return new File { Id = 1, Name = "1", Type = "dir", AccessLink = "we", Size = "0", UserId = 1, ParentId = null, Path = "", Date = DateTime.Now };

        }
        

        #endregion
    }
}
