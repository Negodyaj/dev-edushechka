using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Helpers;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class UserServiceTests
    {

        private Mock<IUserRepository> _repoMock;
        private Mock<IOptions<FilesSettings>> _fileSettingsMock;
        private UserValidationHelper _validationHelper;
        private UserService _sut;
        private Mock<IFileHelper> _workWithFilesMock;
        private const string _folderUserPhotoPath = "/media/userPhoto/";

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IUserRepository>();
            _fileSettingsMock = new Mock<IOptions<FilesSettings>>();
            _validationHelper = new UserValidationHelper(_repoMock.Object);
            _workWithFilesMock = new Mock<IFileHelper>();
            _sut = new UserService(_repoMock.Object, _validationHelper, _fileSettingsMock.Object, _workWithFilesMock.Object, null);
        }

        [Test]
        public async Task AddUser_DtoWithRoles_UserDtoWithRolesCreated()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedUserId = UserData.ExpectedUserId;

            _repoMock.Setup(x => x.AddUserAsync(user)).ReturnsAsync(UserData.ExpectedUserId);
            _repoMock.Setup(x => x.AddUserRoleAsync(UserData.ExpectedUserId, It.IsAny<int>()));
            _repoMock.Setup(x => x.GetUserByIdAsync(expectedUserId)).ReturnsAsync(new UserDto { Id = expectedUserId });

            //When
            var actualDto = await _sut.AddUserAsync(user);

            //Then
            Assert.AreEqual(UserData.ExpectedUserId, actualDto.Id);
            _repoMock.Verify(x => x.AddUserAsync(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRoleAsync(actualDto.Id, It.IsAny<int>()), Times.Once);
        }

        [TestCase (UserData.ExpectedUserId, Role.Manager)]
        [TestCase (UserData.ExpectedUserId, Role.Admin)]
        [TestCase (1, Role.Student)]
        public async Task SelectById_IntUserId_ReturnUserDto(int userId,Role role)
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            var userInfo= UserIdentityInfoData.GetUserIdentityWithRole(role, expectedDto.Id);
            _repoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(expectedDto);

            //When
            var actualDto = await _sut.GetUserByIdAsync(userId, userInfo);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task SelectUserByEmail_UserEmail_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            _repoMock.Setup(x => x.GetUserByEmailAsync(expectedDto.Email)).ReturnsAsync(expectedDto);

            //When
            var actualDto = await _sut.GetUserByEmailAsync(expectedDto.Email);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.GetUserByEmailAsync(expectedDto.Email), Times.Once);
        }

        [Test]
        public async Task SelectUsers_NoEntries_ReturnListUserDto()
        {
            //Given
            var expectedList = UserData.GetListUsersDto();
            _repoMock.Setup(x => x.GetAllUsersAsync()).ReturnsAsync(expectedList);

            //When
            var actualList = await _sut.GetAllUsersAsync();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _repoMock.Verify(x => x.GetAllUsersAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            var expectedAnotherDto = UserData.GetAnotherUserDto();
            var expectedMinimumCallCount = 2;

            _repoMock.Setup(x => x.UpdateUserAsync(expectedAnotherDto));
            _repoMock.Setup(x => x.GetUserByIdAsync(expectedAnotherDto.Id)).ReturnsAsync(expectedDto);

            //When
            var actualDto = await _sut.UpdateUserAsync(expectedAnotherDto);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.UpdateUserAsync(expectedAnotherDto), Times.Once);
            _repoMock.Verify(x => x.GetUserByIdAsync(expectedAnotherDto.Id), Times.AtLeast(expectedMinimumCallCount));
        }

        [Test]
        public async Task AddUser_WhenDtoWithoutRoles_UserDtoWithRoleStudentCreated()
        {
            //Given
            var expectedUser = UserData.GetAnotherUserDto();

            _repoMock.Setup(x => x.AddUserAsync(expectedUser)).ReturnsAsync(UserData.ExpectedUserId);
            _repoMock.Setup(x => x.AddUserRoleAsync(UserData.ExpectedUserId, It.IsAny<int>()));
            _repoMock.Setup(x => x.GetUserByIdAsync(UserData.ExpectedUserId)).ReturnsAsync(expectedUser);

            //When
            var actualUser = await _sut.AddUserAsync(expectedUser);

            //Then
            Assert.AreEqual(expectedUser, actualUser);
            _repoMock.Verify(x => x.AddUserAsync(expectedUser), Times.Once);
            _repoMock.Verify(x => x.AddUserRoleAsync(actualUser.Id, It.IsAny<int>()), Times.Never);
            _repoMock.Verify(x => x.GetUserByIdAsync(UserData.ExpectedUserId), Times.Once);
        }

        [TestCase(1)]
        [TestCase(100)]
        public async Task SelectUserById_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await _sut.GetUserByIdAsync(userId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [TestCase( Role.Student)]
        [TestCase( Role.Methodist)]
        public async Task SelectUserById_WhenDoNotAccessPremission_AuthorizationException( Role role)
        {
            //Given
            var lead = UserData.GetAnotherUserDto();
            var leadInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, lead.Id);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessGetInfoMessage, UserData.ExpectedUserId);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                async () => await _sut.GetUserByIdAsync(UserData.ExpectedUserId, leadInfo));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException)); 
        }

        [Test]
        public async Task SelectUserByEmail_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            var email = user.Email;
            var expectedException = string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetUserByEmailAsync(user.Email));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserByEmailAsync(user.Email), Times.Once);
        }

        [TestCase(5, Role.Student)]
        [TestCase(5, Role.Methodist)]
        public async Task UpdateUser_WhenDoNotAccessPremission_AuthorizationException(int leadId, Role role)
        {
            //Given
            var user = UserData.GetAnotherUserDto();
            var leadInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, leadId);
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessGetInfoMessage, user.Id);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                async () => await _sut.UpdateUserAsync(user, leadInfo));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));

            
        }

        [Test]
        public async Task UpdateUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), user.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateUserAsync(user));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserByIdAsync(user.Id), Times.Once);
            _repoMock.Verify(x => x.UpdateUserAsync(user), Times.Never);
        }

        [TestCase(100)]
        public async Task DeleteUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int id)
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteUserAsync(id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserAsync(id), Times.Never);
        }

        [TestCase(100)]
        public async Task AddUserRole_WhenDoNotHaveMatchesUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = (int)Role.Student;
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddUserRoleAsync(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUserRoleAsync(userId, roleId), Times.Never);
        }

        [TestCase(100)]
        public async Task DeleteUserRole_WhenDoNotHaveMatchesUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = (int)Role.Student;
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteUserRoleAsync(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserRoleAsync(userId, roleId), Times.Never);
        }

        [Test]
        public async Task AddUserRole_UserIdAndRoleId_UserRoleWasCreated()
        {
            //Given
            var roleId = (int)Role.Student;
            var user = UserData.GetUserDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.AddUserRoleAsync(userId, roleId));
            _repoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            //When
            await _sut.AddUserRoleAsync(userId, roleId);

            //Then
            _repoMock.Verify(x => x.AddUserRoleAsync(userId, roleId), Times.Once);
            _repoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task DeleteUserRole_UserIdAndRoleId_UserRoleWasDeleted()
        {
            //Given
            var roleId = (int)Role.Student;
            var user = UserData.GetUserDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.DeleteUserRoleAsync(userId, roleId));
            _repoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            //When
            await _sut.DeleteUserRoleAsync(userId, roleId);

            //Then
            _repoMock.Verify(x => x.DeleteUserRoleAsync(userId, roleId), Times.Once);
            _repoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public async Task DeleteUser_UserId_UserWasDeleted()
        {
            //Given
            var user = UserData.GetUserDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.DeleteUserAsync(userId));
            _repoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);

            //When
            await _sut.DeleteUserAsync(userId);

            //Than
            _repoMock.Verify(x => x.DeleteUserAsync(userId), Times.Once);
            _repoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [TestCase("test.jpg", "/staticFolder/")]
        [TestCase("test.jpeg", "/staticFolder/")]
        [TestCase("test.png", "/staticFolder/")]
        [TestCase("test.png", "/staticFolder")]
        [TestCase("test.png", "/")]
        [TestCase("test.png", "")]
        public async Task ChangeUserPhoto_UserIdAndIFormFile_FileCreatedAndUpdatedInDBTryDeletedOld(string fileName,
            string staticFolderPath)
        {

            //given
            var user = UserData.GetUserDto();
            var userId = user.Id;
            var md5Hash = "sdsdsasd";

            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();

            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            _fileSettingsMock.Setup(x => x.Value).Returns(new FilesSettings() { PathToStaticFolder = staticFolderPath });
            _repoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(user);
            _workWithFilesMock.Setup(x => x.ComputeFileHash(It.IsAny<IFormFile>())).Returns(md5Hash);

            var sbPathToSavePhoto = new StringBuilder();
            sbPathToSavePhoto.Append(staticFolderPath.TrimEnd('/'));
            sbPathToSavePhoto.Append(_folderUserPhotoPath);
            sbPathToSavePhoto.Append(md5Hash);
            sbPathToSavePhoto.Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            sbPathToSavePhoto.Append(Path.GetExtension(fileName));
            var expected = sbPathToSavePhoto.ToString().TrimStart('.');

            //when

            var actual = await _sut.ChangeUserPhotoAsync(userId, fileMock.Object);

            //then 
            Assert.AreEqual(expected, actual);
            _repoMock.Verify(x => x.GetUserByIdAsync(It.IsAny<int>()), Times.Once());
            _workWithFilesMock.Verify(x => x.ComputeFileHash(It.IsAny<IFormFile>()), Times.Once());
            _workWithFilesMock.Verify(x => x.CreateFile(It.IsAny<string>(), It.IsAny<IFormFile>()), Times.Once());
            _workWithFilesMock.Verify(x => x.TryDeleteFile(It.IsAny<string>()), Times.Once());
        }


        [Test]
        public async Task ChangeUserPhoto_WhenUserIsNotFound_EntityNotFoundException()
        {
            //given
            var userId = 42;
            _repoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync((UserDto?)null);
            var fileMock = new Mock<IFormFile>();

            //when then

            Assert.ThrowsAsync<EntityNotFoundException>(() => _sut.ChangeUserPhotoAsync(userId, fileMock.Object));
            _repoMock.Verify(x => x.GetUserByIdAsync(It.IsAny<int>()), Times.Once());
            _workWithFilesMock.Verify(x => x.ComputeFileHash(It.IsAny<IFormFile>()), Times.Never());
            _workWithFilesMock.Verify(x => x.CreateFile(It.IsAny<string>(), It.IsAny<IFormFile>()), Times.Never());
            _workWithFilesMock.Verify(x => x.TryDeleteFile(It.IsAny<string>()), Times.Never());
        }
    }
}