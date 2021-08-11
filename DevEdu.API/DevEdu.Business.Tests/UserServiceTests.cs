using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

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
        public void AddUser_DtoWithRoles_UserDtoWithRolesCreated()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedUserId = UserData.expectedUserId;

            _repoMock.Setup(x => x.AddUser(user)).Returns(UserData.expectedUserId);
            _repoMock.Setup(x => x.AddUserRole(UserData.expectedUserId, It.IsAny<int>()));
            _repoMock.Setup(x => x.SelectUserById(expectedUserId)).Returns(new UserDto { Id = expectedUserId });

            //When
            var actualId = _sut.AddUser(user);

            //Then
            Assert.AreEqual(UserData.expectedUserId, actualId.Id);
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualId.Id, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
        }

        [Test]
        public void SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            _repoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(expectedDto);

            //When
            var actualDto = _sut.SelectUserById(UserData.expectedUserId);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.Once);
        }

        [Test]
        public void SelectUserByEmail_UserEmail_ReturnUserDto()
        {
            //Given
            var user = UserData.GetUserDto();
            _repoMock.Setup(x => x.SelectUserByEmail(user.Email)).Returns(user);

            //When
            var actualUser = _sut.SelectUserByEmail(user.Email);

            //Then
            Assert.AreEqual(user, actualUser);
            _repoMock.Verify(x => x.SelectUserByEmail(user.Email), Times.Once);
        }

        [Test]
        public void SelectUsers_NoEntries_ReturnListUserDto()
        {
            //Given
            var expectedList = UserData.GetListUsersDto();
            _repoMock.Setup(x => x.SelectUsers()).Returns(expectedList);

            //When
            var actualList = _sut.SelectUsers();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _repoMock.Verify(x => x.SelectUsers(), Times.Once);
        }

        [Test]
        public void UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            var expectedAnotherDto = UserData.GetAnotherUserDto();
            var expectedMinimumCallCount = 2;

            _repoMock.Setup(x => x.UpdateUser(expectedDto));
            _repoMock.Setup(x => x.SelectUserById(expectedDto.Id)).Returns(expectedAnotherDto);

            //When
            var actualDto = _sut.UpdateUser(expectedDto);

            //Then
            Assert.AreEqual(expectedAnotherDto, actualDto);
            _repoMock.Verify(x => x.UpdateUser(expectedDto), Times.Once);
            _repoMock.Verify(x => x.SelectUserById(expectedDto.Id), Times.AtLeast(expectedMinimumCallCount));
        }

        [Test]
        public void AddUser_WhenDtoWithoutRoles_UserDtoWithRoleStudentCreated()
        {
            //Given
            var expectedUser = UserData.GetAnotherUserDto();

            _repoMock.Setup(x => x.AddUser(expectedUser)).Returns(UserData.expectedUserId);
            _repoMock.Setup(x => x.AddUserRole(UserData.expectedUserId, It.IsAny<int>()));
            _repoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(expectedUser);

            //When
            var actualUser = _sut.AddUser(expectedUser);

            //Then
            Assert.AreEqual(expectedUser, actualUser);
            _repoMock.Verify(x => x.AddUser(expectedUser), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualUser.Id, It.IsAny<int>()), Times.Never);
            _repoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.Once);
        }

        [TestCase(1)]
        [TestCase(100)]
        public void SelectUserById_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.SelectUserById(userId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void SelectUserByEmail_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            var email = user.Email;
            var expectedException = string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.SelectUserByEmail(user.Email));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.SelectUserByEmail(user.Email), Times.Once);
        }

        [Test]
        public void UpdateUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), user.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.UpdateUser(user));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.SelectUserById(user.Id), Times.Once);
            _repoMock.Verify(x => x.UpdateUser(user), Times.Never);
        }

        [TestCase(100)]
        public void DeleteUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int id)
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteUser(id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUser(id), Times.Never);
        }

        [TestCase(100)]
        public void AddUserRole_WhenDoNotHaveMatchesUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = 6;
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Never);
        }

        [TestCase(100)]
        public void DeleteUserRole_WhenDoNotHaveMatchesUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = 6;
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Never);
        }
       
        [Test]
        public void AddUserRole_UserIdAndRoleId_UserRoleWasCreated()
        {
            //Given
            var roleId = 6;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            _repoMock.Setup(x => x.AddUserRole(userId, roleId));
            _repoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            _sut.AddUserRole(userId, roleId);

            //Then
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Once);
            _repoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteUserRole_UserIdAndRoleId_UserRoleWasDeleted()
        {
            //Given
            var roleId = 6;
            var user = UserData.GetUserDto();
            var userId = user.Id;
            _repoMock.Setup(x => x.DeleteUserRole(userId, roleId));
            _repoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            _sut.DeleteUserRole(userId, roleId);

            //Then
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Once);
            _repoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteUser_UserId_UserWasDeleted()
        {
            //Given
            var user = UserData.GetUserDto();
            var userId = user.Id;
            _repoMock.Setup(x => x.DeleteUser(userId));
            _repoMock.Setup(x => x.SelectUserById(userId)).Returns(user);

            //When
            _sut.DeleteUser(userId);

            //Than
            _repoMock.Verify(x => x.DeleteUser(userId), Times.Once);
            _repoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }
    }
}