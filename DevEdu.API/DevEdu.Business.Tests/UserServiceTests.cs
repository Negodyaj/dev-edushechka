using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests.Services
{
    public class UserServiceTests
    {

        private Mock<IUserRepository> _repoMock;
        private UserValidationHelper _validationHelper;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IUserRepository>();
            _validationHelper = new UserValidationHelper(_repoMock.Object);
            _userService = new UserService(_repoMock.Object, _validationHelper);
        }

        [Test]
        public void AddUser_DtoWithRoles_UserDtoWithRolesCreated()
        {
            //Given
            var user = UserData.GetUserDto();

            _repoMock.Setup(x => x.AddUser(user)).Returns(UserData.expectedUserId);
            _repoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(user);
            _repoMock.Setup(x => x.AddUserRole(UserData.expectedUserId, It.IsAny<int>()));

            //When
            var actualId = _userService.AddUser(user);

            //Then
            Assert.AreEqual(UserData.expectedUserId, actualId);
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.AtLeastOnce);
            _repoMock.Verify(x => x.AddUserRole(actualId, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
        }

        [Test]
        public void SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            _repoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(expectedDto);

            //When
            var actualDto = _userService.SelectUserById(UserData.expectedUserId);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _repoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.AtLeastOnce);
        }

        [Test]
        public void SelectUserByEmail_UserEmail_ReturnUserDto()
        {
            //Given
            var user = UserData.GetUserDto();
            _repoMock.Setup(x => x.SelectUserByEmail(user.Email)).Returns(user);

            //When
            var actualUser = _userService.SelectUserByEmail(user.Email);

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
            var actualList = _userService.SelectUsers();

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
            var actualDto = _userService.UpdateUser(expectedDto);

            //Then
            Assert.AreEqual(expectedAnotherDto, actualDto);
            _repoMock.Verify(x => x.UpdateUser(expectedDto), Times.Once);
            _repoMock.Verify(x => x.SelectUserById(expectedDto.Id), Times.AtLeast(expectedMinimumCallCount));
        }

        [Test]
        public void AddUser_WhenDtoWithoutRoles_UserDtoWithRoleStudentCreated()
        {
            //Given
            var user = UserData.GetAnotherUserDto();

            _repoMock.Setup(x => x.AddUser(user)).Returns(UserData.expectedUserId);
            _repoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(user);
            _repoMock.Setup(x => x.AddUserRole(UserData.expectedUserId, It.IsAny<int>()));

            //When
            var actualId = _userService.AddUser(user);

            //Then
            Assert.AreEqual(UserData.expectedUserId, actualId);
            _repoMock.Verify(x => x.AddUser(user), Times.Once);
            _repoMock.Verify(x => x.AddUserRole(actualId, It.IsAny<int>()), Times.AtLeastOnce);
            _repoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.AtLeastOnce);
        }


        [TestCase(0)]
        [TestCase(-100)]
        public void SelectUserById_WhenIdLessThanAllowedValue_Exception(int id)
        {
            //Given
            var expectedException = string.Format(ServiceMessages.MinimumAllowedValueMessage, nameof(id), UserData.idMinimum);

            //When
            var ex = Assert.Throws<Exception>(
                () => _userService.SelectUserById(id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.SelectUserById(id), Times.Never);
        }

        [Test]
        public void SelectUserByEmail_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = $"{nameof(user)} with email = {user.Email} was not found";

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _userService.SelectUserByEmail(user.Email));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.SelectUserByEmail(user.Email), Times.Once);
        }

        [Test]
        public void SelectUsers_WhenListIsEmpty_Exception()
        {
            //Given
            var list = _repoMock.Setup(x => x.SelectUsers()).Returns(new List<UserDto>() { });
            var expectedException = $"{nameof(list)} is empty";

            //When
            var ex = Assert.Throws<Exception>(
                () => _userService.SelectUsers());

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.SelectUsers(), Times.Once);
        }

        [Test]
        public void UpdateUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), user.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _userService.UpdateUser(user));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.SelectUserById(user.Id), Times.Once);
            _repoMock.Verify(x => x.UpdateUser(user), Times.Never);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100500)]
        public void DeleteUser_WhenDoNotHaveMatchesInDataBase_EntityNotFoundException(int id)
        {
            //Given
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _userService.DeleteUser(id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUser(id), Times.Never);
        }

        [TestCase(0)]
        [TestCase(-100500)]
        public void DeleteUser_WhenIdLessThanMinimumAllowedValue_Exception(int id)
        {
            //Given
            var expectedException = string.Format(ServiceMessages.MinimumAllowedValueMessage, nameof(id), UserData.idMinimum);

            //When
            var ex = Assert.Throws<Exception>(
                () => _userService.DeleteUser(id));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUser(id), Times.Never);
        }

        [TestCase(0, 0)]
        [TestCase(-100500, 0)]
        [TestCase(0, -100500)]
        public void AddUserRole_WhenIdLessThanMinimumAllowedValue_Exception(int userId, int roleId)
        {
            //Given
            var expectedException = string.Format(ServiceMessages.MinimumAllowedValueMessage, nameof(userId), nameof(roleId), UserData.idMinimum);

            //When
            var ex = Assert.Throws<Exception>(
                () => _userService.AddUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Never);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100500)]
        public void AddUserRole_WhenDoNotHaveMatchesUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = 6;
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _userService.AddUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Never);
        }

        [TestCase(7)]
        [TestCase(100)]
        [TestCase(100500)]
        public void AddUserRole_WhenDoNotHaveMatchesRoleIdInDataBase_EntityNotFoundException(int roleId)
        {
            //Given
            var user = UserData.GetUserDto();
            var role = Enum.GetName(typeof(Role), roleId);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(role), roleId);

            _repoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(user);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _userService.AddUserRole(UserData.expectedUserId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.AddUserRole(UserData.expectedUserId, roleId), Times.Never);
            _repoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.AtLeastOnce);
        }

        [TestCase(0, 0)]
        [TestCase(-100500, 0)]
        [TestCase(0, -100500)]
        public void DeleteUserRole_WhenIdLessThanMinimumAllowedValue_Exception(int userId, int roleId)
        {
            //Given
            var expectedException = string.Format(ServiceMessages.MinimumAllowedValueMessage, nameof(userId), nameof(roleId), UserData.idMinimum);

            //When
            var ex = Assert.Throws<Exception>(
                () => _userService.DeleteUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Never);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100500)]
        public void DeleteUserRole_WhenDoNotHaveMatchesUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = 6;
            var user = UserData.GetUserDto();
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(user), userId);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _userService.DeleteUserRole(userId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Never);
        }

        [TestCase(7)]
        [TestCase(100)]
        [TestCase(100500)]
        public void DeleteUserRole_WhenDoNotHaveMatchesRoleIdInDataBase_EntityNotFoundException(int roleId)
        {
            var user = UserData.GetUserDto();
            var role = Enum.GetName(typeof(Role), roleId);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(role), roleId);
           
            _repoMock.Setup(x => x.SelectUserById(UserData.expectedUserId)).Returns(user);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _userService.DeleteUserRole(UserData.expectedUserId, roleId));

            //Then
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _repoMock.Verify(x => x.DeleteUserRole(UserData.expectedUserId, roleId), Times.Never);
        }
    }
}