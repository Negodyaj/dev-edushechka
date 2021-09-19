using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class UserServiceTests
    {

        private Mock<IUserRepository> _repoMock;
        private UserValidationHelper _validationHelper;
        private UserService _sut;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IUserRepository>();
            _validationHelper = new UserValidationHelper(_repoMock.Object);
            _sut = new UserService(_repoMock.Object, _validationHelper);
        }

        [Test]
        public async Task AddUser_DtoWithRoles_UserDtoWithRolesCreated()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedUserId = UserData.ExpectedUserId;

            _repoMock.Setup(x => x.AddUser(user)).ReturnsAsync(UserData.ExpectedUserId);
            _repoMock.Setup(x => x.AddUserRole(UserData.ExpectedUserId, It.IsAny<int>()));
            _repoMock.Setup(x => x.GetUserById(expectedUserId)).ReturnsAsync(new UserDto { Id = expectedUserId });

            //When
            var actualDto = await _sut.AddUser(user);

            //Then
            Assert.AreEqual(UserData.ExpectedUserId, actualDto.Id);
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualDto.Id, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
        }

        [Test]
        public async Task SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            _repoMock.Setup(x => x.GetUserById(UserData.ExpectedUserId)).ReturnsAsync(expectedDto);

            //When
            var actualDto = await _sut.GetUserById(UserData.ExpectedUserId);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.GetUserById(UserData.ExpectedUserId), Times.Once);
        }

        [Test]
        public async Task SelectUserByEmail_UserEmail_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            _repoMock.Setup(x => x.GetUserByEmail(expectedDto.Email)).ReturnsAsync(expectedDto);

            //When
            var actualDto = await _sut.GetUserByEmail(expectedDto.Email);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.GetUserByEmail(expectedDto.Email), Times.Once);
        }

        [Test]
        public async Task SelectUsers_NoEntries_ReturnListUserDto()
        {
            //Given
            var expectedList = UserData.GetListUsersDto();
            _repoMock.Setup(x => x.GetAllUsers()).ReturnsAsync(expectedList);

            //When
            var actualList = await _sut.GetAllUsers();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _repoMock.Verify(x => x.GetAllUsers(), Times.Once);
        }

        [Test]
        public async Task UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            var expectedAnotherDto = UserData.GetAnotherUserDto();
            var expectedMinimumCallCount = 2;

            _repoMock.Setup(x => x.UpdateUser(expectedAnotherDto));
            _repoMock.Setup(x => x.GetUserById(expectedAnotherDto.Id)).ReturnsAsync(expectedDto);

            //When
            var actualDto = await _sut.UpdateUser(expectedAnotherDto);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.UpdateUser(expectedAnotherDto), Times.Once);
            _repoMock.Verify(x => x.GetUserById(expectedAnotherDto.Id), Times.AtLeast(expectedMinimumCallCount));
        }

        [Test]
        public async Task AddUser_WhenDtoWithoutRoles_UserDtoWithRoleStudentCreated()
        {
            //Given
            var expectedUser = UserData.GetAnotherUserDto();

            _repoMock.Setup(x => x.AddUser(expectedUser)).ReturnsAsync(UserData.ExpectedUserId);
            _repoMock.Setup(x => x.AddUserRole(UserData.ExpectedUserId, It.IsAny<int>()));
            _repoMock.Setup(x => x.GetUserById(UserData.ExpectedUserId)).ReturnsAsync(expectedUser);

            //When
            var actualUser = await _sut.AddUser(expectedUser);

            //Then
            Assert.AreEqual(expectedUser, actualUser);
            _repoMock.Verify(x => x.AddUser(expectedUser), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualUser.Id, It.IsAny<int>()), Times.Never);
            _repoMock.Verify(x => x.GetUserById(UserData.ExpectedUserId), Times.Once);
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
                async () => await _sut.GetUserById(userId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
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
                () => _sut.GetUserByEmail(user.Email));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserByEmail(user.Email), Times.Once);
        }

        [Test]
        public async Task UpdateUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), user.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateUser(user));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserById(user.Id), Times.Once);
            _repoMock.Verify(x => x.UpdateUser(user), Times.Never);
        }

        [TestCase(100)]
        public async Task DeleteUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int id)
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteUser(id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUser(id), Times.Never);
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
                () => _sut.AddUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Never);
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
                () => _sut.DeleteUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Never);
        }

        [Test]
        public async Task AddUserRole_UserIdAndRoleId_UserRoleWasCreated()
        {
            //Given
            var roleId = (int)Role.Student;
            var user = UserData.GetUserDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.AddUserRole(userId, roleId));
            _repoMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(user);

            //When
            await _sut.AddUserRole(userId, roleId);

            //Then
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Once);
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public async Task DeleteUserRole_UserIdAndRoleId_UserRoleWasDeleted()
        {
            //Given
            var roleId = (int)Role.Student;
            var user = UserData.GetUserDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.DeleteUserRole(userId, roleId));
            _repoMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(user);

            //When
            await _sut.DeleteUserRole(userId, roleId);

            //Then
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Once);
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public async Task DeleteUser_UserId_UserWasDeleted()
        {
            //Given
            var user = UserData.GetUserDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.DeleteUser(userId));
            _repoMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(user);

            //When
            await _sut.DeleteUser(userId);

            //Than
            _repoMock.Verify(x => x.DeleteUser(userId), Times.Once);
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }
    }
}