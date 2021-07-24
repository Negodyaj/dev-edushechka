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
        private Mock<IUserValidationHelper> _validationHelperMock;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IUserRepository>();
            _validationHelperMock = new Mock<IUserValidationHelper>();
            _userService = new UserService(_repoMock.Object, _validationHelperMock.Object);
            var expectedDto = UserData.GetUserDto();

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
            _repoMock.Verify(x => x.SelectUserById(UserData.expectedUserId), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-100)]
        public void SelectUserById_WhenIdWrongFormat_Exception(int userId)
        {
            //Given
            _repoMock.Setup(x => x.SelectUserById(userId)).Throws(new Exception(""));

            //When
            try
            {
                _userService.SelectUserById(userId);
            }
            catch
            {
                Assert.Pass($"{userId} less then 0");
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void SelectUserByEmail()
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
        public void SelectUserByEmail_WhenDoNotHaveUserIdInDataBase_EntityNotFoundException()
        {
            //Given
            var user = UserData.GetUserDto();
            _repoMock.Setup(x => x.SelectUserByEmail(user.Email)).Throws(new EntityNotFoundException(""));

            //When
            try
            {
                _userService.SelectUserByEmail(user.Email);
            }
            catch (EntityNotFoundException)
            {
                Assert.Pass($"{nameof(user)} with email = {user.Email} was not found");
            }

            //Then
            Assert.Fail();
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
        public void SelectUsers_WhenListIsEmpty_Exception()
        {
            //Given
            var list = _repoMock.Setup(x => x.SelectUsers()).Returns(new List<UserDto>() { null });
            _repoMock.Setup(x => x.SelectUsers()).Throws(new Exception(""));

            //When
            try
            {
                _userService.SelectUsers();
            }
            catch
            {
                Assert.Pass($"{nameof(list)} is empty");
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.SelectUsers(), Times.Once);
        }

        [Test]
        public void UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var expectedDto = UserData.GetUserDto();
            var expectedAnotherDto = UserData.GetAnotherUserDto();

            _repoMock.Setup(x => x.UpdateUser(expectedDto));
            _repoMock.Setup(x => x.SelectUserById(expectedDto.Id)).Returns(expectedAnotherDto);

            //When
            var actualDto = _userService.UpdateUser(expectedDto);

            //Then
            Assert.AreEqual(expectedAnotherDto, actualDto);
            _repoMock.Verify(x => x.UpdateUser(expectedDto), Times.Once);
            _repoMock.Verify(x => x.SelectUserById(expectedDto.Id), Times.Once);
        }

        [Test]
        public void UpdateUser_WhenDoNotHaveUserIdInDataBase_EntityNotFoundException()
        {
            //Given
            var dto = UserData.GetUserDto();
            _repoMock.Setup(x => x.UpdateUser(dto)).Throws(new EntityNotFoundException(""));

            //When
            try
            {
                _userService.UpdateUser(dto);
            }
            catch (EntityNotFoundException)
            {
                Assert.Pass(string.Format(ServiceMessages.EntityNotFoundMessage, typeof(UserDto), dto.Id));
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.UpdateUser(dto), Times.Once);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100500)]
        public void DeleteUser_WhenDoNotHaveUserIdInDataBase_EntityNotFoundException(int id)
        {
            //Given
            _repoMock.Setup(x => x.DeleteUser(id)).Throws(new EntityNotFoundException(""));

            //When
            try
            {
                _userService.DeleteUser(id);
            }
            catch (EntityNotFoundException)
            {
                Assert.Pass(string.Format(ServiceMessages.EntityNotFoundMessage, typeof(UserDto), id));
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.DeleteUser(id), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-100500)]
        public void DeleteUser_WhenIdLessThanZero_Exception(int id)
        {
            //Given
            _repoMock.Setup(x => x.DeleteUser(id)).Throws(new Exception(""));

            //When
            try
            {
                _userService.DeleteUser(id);
            }
            catch
            {
                Assert.Pass($"{nameof(id)} less then 0");
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.DeleteUser(id), Times.Once);
        }

        [TestCase(0, 0)]
        [TestCase(-100500, 0)]
        [TestCase(0, -100500)]
        public void AddUserRole_WhenIdLessThanZero_Exception(int userId, int roleId)
        {
            //Given
            _repoMock.Setup(x => x.AddUserRole(userId, roleId)).Throws(new Exception(""));

            //When
            try
            {
                _userService.AddUserRole(userId, roleId);
            }
            catch
            {
                Assert.Pass($"{nameof(userId)} or {nameof(roleId)} less then 0");
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Once);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100500)]
        public void AddUserRole_WhenDoNotHaveUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = 6;
            _repoMock.Setup(x => x.AddUserRole(userId, roleId)).Throws(new EntityNotFoundException(""));

            //When
            try
            {
                _userService.AddUserRole(userId, roleId);
            }
            catch (EntityNotFoundException)
            {
                Assert.Pass(string.Format(ServiceMessages.EntityNotFoundMessage, typeof(UserDto), userId));
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Once);
        }

        [TestCase(0)]
        [TestCase(100)]
        [TestCase(100500)]
        public void AddUserRole_WhenDoNotHaveRoleIdInEnum_EntityNotFoundException(int roleId)
        {
            //Given
            var userId = 1;
            _repoMock.Setup(x => x.AddUserRole(userId, roleId)).Throws(new EntityNotFoundException(""));

            //When
            try
            {
                _userService.AddUserRole(userId, roleId);
            }
            catch (EntityNotFoundException)
            {
                Assert.Pass(string.Format(ServiceMessages.EntityNotFoundMessage, typeof(Role), roleId));
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.AddUserRole(userId, roleId), Times.Once);
        }

        [TestCase(0, 0)]
        [TestCase(-100500, 0)]
        [TestCase(0, -100500)]
        public void DeleteUserRole_WhenIdLessThanZero_Exception(int userId, int roleId)
        {
            //Given
            _repoMock.Setup(x => x.DeleteUserRole(userId, roleId)).Throws(new Exception(""));

            //When
            try
            {
                _userService.DeleteUserRole(userId, roleId);
            }
            catch
            {
                Assert.Pass($"{nameof(userId)} or {nameof(roleId)} less then 0");
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Once);
        }

        [TestCase(1)]
        [TestCase(100)]
        [TestCase(100500)]
        public void DeleteUserRole_WhenDoNotHaveUserIdInDataBase_EntityNotFoundException(int userId)
        {
            //Given
            var roleId = 6;
            _repoMock.Setup(x => x.DeleteUserRole(userId, roleId)).Throws(new EntityNotFoundException(""));

            //When
            try
            {
                _userService.DeleteUserRole(userId, roleId);
            }
            catch (EntityNotFoundException)
            {
                Assert.Pass(string.Format(ServiceMessages.EntityNotFoundMessage, typeof(UserDto), userId));
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Once);
        }

        [TestCase(0)]
        [TestCase(100)]
        [TestCase(100500)]
        public void DeleteUserRole_WhenDoNotHaveRoleIdInEnum_EntityNotFoundException(int roleId)
        {
            //Given
            var userId = 1;
            _repoMock.Setup(x => x.DeleteUserRole(userId, roleId)).Throws(new EntityNotFoundException(""));

            //When
            try
            {
                _userService.DeleteUserRole(userId, roleId);
            }
            catch (EntityNotFoundException)
            {
                Assert.Pass(string.Format(ServiceMessages.EntityNotFoundMessage, typeof(Role), roleId));
            }

            //Then
            Assert.Fail();
            _repoMock.Verify(x => x.DeleteUserRole(userId, roleId), Times.Once);
        }
    }
}