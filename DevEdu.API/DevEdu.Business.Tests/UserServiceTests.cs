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

        [Test]
        public async Task SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            _repoMock.Setup(x => x.GetUserByIdAsync(UserData.ExpectedUserId)).ReturnsAsync(expectedDto);

            //When
            var actualDto = await _sut.GetUserByIdAsync(UserData.ExpectedUserId);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.GetUserByIdAsync(UserData.ExpectedUserId), Times.Once);
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
    }
}