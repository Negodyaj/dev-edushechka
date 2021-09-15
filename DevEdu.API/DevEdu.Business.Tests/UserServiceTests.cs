using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
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
        public void AddUser_DtoWithRolesAdminToken_UserDtoWithRolesStudentAndAdminCreated()
        {
            //Given
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedUserId = UserData.ExpectedUserId;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();

            _repoMock.Setup(x => x.AddUser(user)).Returns(UserData.ExpectedUserId);
            _repoMock.Setup(x => x.AddUserRole(UserData.ExpectedUserId, It.IsAny<Role>()));
            _repoMock.Setup(x => x.GetUserById(expectedUserId)).Returns(new UserDto { Id = expectedUserId });

            //When
            var actualDto = _sut.AddUser(user, userInfo);

            //Then
            Assert.AreEqual(UserData.ExpectedUserId, actualDto.Id);
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualDto.Id, It.IsAny<Role>()), Times.Exactly(user.Roles.Count)); 
            _repoMock.Verify(x => x.GetUserById(expectedUserId), Times.Once);
        }

        [Test]
        public void AddUser_DtoWithRolesAdminToken_UserDtoWithRolesStudentCreated()
        {
            //Given
            var user = UserData.GetUserWithEmptyRolesDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();

            _repoMock.Setup(x => x.AddUser(user)).Returns(user.Id);
            _repoMock.Setup(x => x.AddUserRole(user.Id, It.IsAny<Role>()));
            _repoMock.Setup(x => x.GetUserById(user.Id)).Returns(new UserDto { Id = user.Id });

            //When
            var actualDto = _sut.AddUser(user, userInfo);

            //Then
            Assert.AreEqual(user.Id, actualDto.Id);
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualDto.Id, It.IsAny<Role>()), Times.Once); 
            _repoMock.Verify(x => x.GetUserById(user.Id), Times.Once);
        }

        [Test]
        public void AddUser_DtoWithRolesManagerToken_UserDtoWithRoleStudentCreated()
        {
            //Given
            var user = UserData.GetStudentUserDto();
            var expectedUserId = UserData.ExpectedUserId;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();

            _repoMock.Setup(x => x.AddUser(user)).Returns(UserData.ExpectedUserId);
            _repoMock.Setup(x => x.AddUserRole(UserData.ExpectedUserId, It.IsAny<Role>()));
            _repoMock.Setup(x => x.GetUserById(expectedUserId)).Returns(new UserDto { Id = expectedUserId });

            //When
            var actualDto = _sut.AddUser(user, userInfo);

            //Then
            Assert.AreEqual(UserData.ExpectedUserId, actualDto.Id);
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(
                actualDto.Id, It.IsAny<Role>()), Times.Exactly(user.Roles.Count - 1)); // -1 because Role.Student add automatically
        }

        [Test]
        public void AddUser_DtoWithRolesManagerToken_AuthorizationExceptionException()
        {
            //Given
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedException = string.Format(ServiceMessages.AdminCanAddRolesToUserMessage, nameof(Role.Admin));
            var expectedUserId = UserData.ExpectedUserId;
            var userInfo = UserIdentityInfoData.GetUserIdentityWithManagerRole();

            _repoMock.Setup(x => x.AddUserRole(UserData.ExpectedUserId, It.IsAny<Role>()));

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.AddUser(user, userInfo));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(user.Id, Role.Admin), Times.Never);
            _repoMock.Verify(x => x.GetUserById(user.Id), Times.Never);
        }

        [Test]
        public void SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserWithRolesStudentAndAdminDto();

            _repoMock.Setup(x => x.GetUserById(UserData.ExpectedUserId)).Returns(expectedDto);

            //When
            var actualDto = _sut.GetUserById(UserData.ExpectedUserId);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.GetUserById(UserData.ExpectedUserId), Times.Once);
        }

        [Test]
        public void SelectUserByEmail_UserEmail_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserWithRolesStudentAndAdminDto();

            _repoMock.Setup(x => x.GetUserByEmail(expectedDto.Email)).Returns(expectedDto);

            //When
            var actualDto = _sut.GetUserByEmail(expectedDto.Email);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.GetUserByEmail(expectedDto.Email), Times.Once);
        }

        [Test]
        public void SelectUsers_NoEntries_ReturnListUserDto()
        {
            //Given
            var expectedList = UserData.GetListUsersDto();

            _repoMock.Setup(x => x.GetAllUsers()).Returns(expectedList);

            //When
            var actualList = _sut.GetAllUsers();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _repoMock.Verify(x => x.GetAllUsers(), Times.Once);
        }

        [Test]
        public void UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedAnotherDto = UserData.GetStudentUserDto();
            var expectedMinimumCallCount = 2;

            _repoMock.Setup(x => x.UpdateUser(expectedAnotherDto));
            _repoMock.Setup(x => x.GetUserById(expectedAnotherDto.Id)).Returns(expectedDto);

            //When
            var actualDto = _sut.UpdateUser(expectedAnotherDto);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.UpdateUser(expectedAnotherDto), Times.Once);
            _repoMock.Verify(x => x.GetUserById(expectedAnotherDto.Id), Times.AtLeast(expectedMinimumCallCount));
        }

        [Test]
        public void AddUser_WhenDtoWithoutRoles_UserDtoWithRoleStudentCreated()
        {
            //Given
            var expectedUser = UserData.GetStudentUserDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithAdminRole();

            _repoMock.Setup(x => x.AddUser(expectedUser)).Returns(UserData.ExpectedUserId);
            _repoMock.Setup(x => x.AddUserRole(UserData.ExpectedUserId, It.IsAny<Role>()));
            _repoMock.Setup(x => x.GetUserById(UserData.ExpectedUserId)).Returns(expectedUser);

            //When
            var actualUser = _sut.AddUser(expectedUser, userInfo);

            //Then
            Assert.AreEqual(expectedUser, actualUser);
            _repoMock.Verify(x => x.AddUser(expectedUser), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualUser.Id, It.IsAny<Role>()), Times.Never);
            _repoMock.Verify(x => x.GetUserById(UserData.ExpectedUserId), Times.Once);
        }

        [TestCase(1)]
        [TestCase(100)]
        public void SelectUserById_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedException = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetUserById(userId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void SelectUserByEmail_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var email = user.Email;
            var expectedException = string.Format(ServiceMessages.EntityWithEmailNotFoundMessage, nameof(user), email);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetUserByEmail(user.Email));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserByEmail(user.Email), Times.Once);
        }

        [Test]
        public void UpdateUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedException = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(user), user.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.UpdateUser(user));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.GetUserById(user.Id), Times.Once);
            _repoMock.Verify(x => x.UpdateUser(user), Times.Never);
        }

        [TestCase(100)]
        public void DeleteUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int id)
        {
            //Given
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedException = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(user), id);

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
            var role = Role.Student;
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedException = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddUserRole(userId, role));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUserRole(userId, role), Times.Never);
        }

        [TestCase(100)]
        public void DeleteUserRole_WhenDoNotHaveMatchesUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var role = Role.Student;
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var expectedException = string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteUserRole(userId, role));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserRole(userId, role), Times.Never);
        }

        [Test]
        public void AddUserRole_UserIdAndRoleId_UserRoleWasCreated()
        {
            //Given
            var role = Role.Student;
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.AddUserRole(userId, role));
            _repoMock.Setup(x => x.GetUserById(userId)).Returns(user);

            //When
            _sut.AddUserRole(userId, role);

            //Then
            _repoMock.Verify(x => x.AddUserRole(userId, role), Times.Once);
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteUserRole_UserIdAndRoleId_UserRoleWasDeleted()
        {
            //Given
            var role = Role.Student;
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var userId = user.Id;
            _repoMock.Setup(x => x.DeleteUserRole(userId, role));
            _repoMock.Setup(x => x.GetUserById(userId)).Returns(user);

            //When
            _sut.DeleteUserRole(userId, role);

            //Then
            _repoMock.Verify(x => x.DeleteUserRole(userId, role), Times.Once);
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void DeleteUser_UserId_UserWasDeleted()
        {
            //Given
            var user = UserData.GetUserWithRolesStudentAndAdminDto();
            var userId = user.Id;

            _repoMock.Setup(x => x.DeleteUser(userId));
            _repoMock.Setup(x => x.GetUserById(userId)).Returns(user);

            //When
            _sut.DeleteUser(userId);

            //Than
            _repoMock.Verify(x => x.DeleteUser(userId), Times.Once);
            _repoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }
    }
}