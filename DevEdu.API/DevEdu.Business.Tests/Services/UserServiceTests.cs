using DevEdu.Business.Servicies;
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

        private Mock<IUserRepository> _userRepoMock;
        private UserDto _userDto;
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _userService = new UserService(_userRepoMock.Object);
            _userDto = new UserDto
            {
                FirstName = "Admin",
                LastName = "Adminov",
                Patronymic = "Adminovich",
                Email = "admin@admin.ad",
                Username = "Admin01",
                Password = "qwerty12345",
                ContractNumber = "admin01",
                City = (City)1,
                BirthDate = DateTime.Today,
                GitHubAccount = "admin/admin.git",
                Photo = "https://localhost:Admin/admin",
                PhoneNumber = "adminPhoneNumber",
                Roles = new List<Role>
                {
                     Role.Student ,
                     Role.Admin
                }
            };
        }

        [Test]
        public void AddUser_DtoWithRoles_UserDtoWithRolesCreated()
        {
            //Given
            var expectedUserId = 1;

            _userRepoMock.Setup(x => x.AddUser(_userDto))
                .Returns(expectedUserId)
                .Verifiable();

            //When
            var actualUserId = _userService.AddUser(_userDto);

            //Then
            Assert.AreEqual(expectedUserId, actualUserId);
            _userRepoMock.Verify(x => x.AddUser(_userDto), Times.Once);
        }

        [Test]
        public void AddUser_WhenDtoIsEmpty_Exception()
        {
            //Given
            UserDto emptyUserDto = null;
            _userRepoMock.Setup(x => x.AddUser(emptyUserDto))
                .Throws(new Exception());

            //When
            try
            {
                _userService.AddUser(emptyUserDto);
            }
            catch
            {
                Assert.Pass();
            }

            //Then
            Assert.Fail();
        }

        [Test]
        public void SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var userId = 1;

            _userRepoMock.Setup(x => x.SelectUserById(userId))
                .Returns(_userDto)
                .Verifiable();

            //When
            var dto = _userService.SelectUserById(userId);

            //Then
            Assert.AreEqual(_userDto, dto);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(-int.MaxValue)]
        public void SelectById_WhenUserIdWrongValue_Exception(int userId)
        {
            //Given
            _userRepoMock.Setup(x => x.SelectUserById(userId))
                .Throws(new Exception());

            //When
            try
            {
                _userService.SelectUserById(userId);
            }
            catch
            {
                Assert.Pass();
            }

            //Then
            Assert.Fail();
        }

        [Test]
        public void SelectUsers_NoEntryes_ReturnListUserDto()
        {
            //Given
            var expectedListUserDto = new List<UserDto>
            {
                _userDto,
                new UserDto
                {
                    FirstName = "Student",
                    LastName = "Studentov",
                    Patronymic = "Studentovich",
                    Email = "student@student.st",
                    Username = "Student01",
                    Password = "qwerty12345",
                    ContractNumber = "Student01",
                    City = (City)1,
                    BirthDate = DateTime.Today,
                    GitHubAccount = "Student/Student.git",
                    Photo = "https://localhost:Student",
                    PhoneNumber = "StudentPhoneNumber",
                    Roles = new List<Role> { Role.Student }
                },
                new UserDto
                {
                    FirstName = "Manager",
                    LastName = "Managerov",
                    Patronymic = "Managerovich",
                    Email = "Manager@manager.mn",
                    Username = "SManager01",
                    Password = "qwerty12345",
                    ContractNumber = "Manager01",
                    City = (City)1,
                    BirthDate = DateTime.Today,
                    GitHubAccount = "Manager/Manager.git",
                    Photo = "https://localhost:Manager",
                    PhoneNumber = "ManagerPhoneNumber",
                    Roles = new List<Role> { Role.Manager }
                }
            };

            _userRepoMock.Setup(x => x.SelectUsers())
                .Returns(expectedListUserDto)
                .Verifiable();

            //When
            var actualListUserDto = _userService.SelectUsers();

            //Then
            Assert.AreEqual(expectedListUserDto, actualListUserDto);
            _userRepoMock.Verify(x => x.SelectUsers(), Times.Once);
        }

        [Test]
        public void UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var expectedUserDto = new UserDto
            {
                FirstName = "Student",
                LastName = "Studentov",
                Patronymic = "Studentovich",
                Email = "student@student.st",
                Username = "Student01",
                Password = "qwerty12345",
                ContractNumber = "Student01",
                City = (City)1,
                BirthDate = DateTime.Today,
                GitHubAccount = "Student/Student.git",
                Photo = "https://localhost:Student",
                PhoneNumber = "StudentPhoneNumber",
                Roles = new List<Role> { Role.Student }
            };

            _userRepoMock.Setup(x => x.UpdateUser(_userDto))
                .Returns(expectedUserDto)
                .Verifiable();

            //When
            var actualUserDto = _userService.UpdateUser(_userDto);

            //Then
            Assert.AreEqual(expectedUserDto, actualUserDto);
            _userRepoMock.Verify(x => x.UpdateUser(_userDto), Times.Once);
        }

        [Test]
        public void UpdateUser_WhenDtoIsEmpty_Exception()
        {
            //Given
            UserDto emptyDto = null;
            _userRepoMock.Setup(x => x.UpdateUser(emptyDto))
                .Throws(new Exception());

            //When
            try
            {
                _userService.UpdateUser(emptyDto);
            }
            catch
            {
                Assert.Pass();
            }

            //Then
            Assert.Fail();
        }

        [TestCase(0)]
        [TestCase(-10)]
        [TestCase(-int.MaxValue)]
        public void DeleteUser_WrongValue_Exception(int userId)
        {
            //Given
            _userRepoMock.Setup(x => x.DeleteUser(userId))
                .Throws(new Exception());

            //When
            try
            {
                _userService.DeleteUser(userId);
            }
            catch
            {
                Assert.Pass();
            }

            //Then
            Assert.Fail();
        }

        [Test]
        public void AddRoleToUser_IntUserIdAndRoleId_ReturnIntRecord()
        {
            //Given
            var expectedUserRoleId = 24;
            _userRepoMock.Setup(x => x.AddUserRole(_userDto.Id, (int)Role.Teacher))
                .Returns(expectedUserRoleId)
                .Verifiable();

            //When
            var actualUserRoleId = _userService.AddUserRole(_userDto.Id, (int)Role.Teacher);

            //Then
            Assert.AreEqual(expectedUserRoleId, actualUserRoleId);
            _userRepoMock.Verify(x => x.AddUserRole(_userDto.Id, (int)Role.Teacher), Times.Once);
        }

        [TestCase(0,1)]
        [TestCase(1,0)]
        [TestCase(-int.MaxValue,-int.MaxValue)]
        public void AddRoleToUser_WrongValue_Exception(int userId,int roleId)
        {
            //Given
            _userRepoMock.Setup(x => x.AddUserRole(userId, roleId))
                .Throws(new Exception());

            //When
            try
            {
                _userService.AddUserRole(userId, roleId);
            }
            catch
            {
                Assert.Pass();
            }

            //Then
            Assert.Fail();
        }

        [TestCase(0, 1)]
        [TestCase(1, 0)]
        [TestCase(-int.MaxValue, -int.MaxValue)]
        public void DeleteUserRole_WrongValue_Exception(int userId, int roleId)
        {
            //Given
            _userRepoMock.Setup(x => x.DeleteUserRole(userId, roleId))
                .Throws(new Exception());

            //When
            try
            {
                _userService.DeleteUserRole(userId, roleId);
            }
            catch
            {
                Assert.Pass();
            }

            //Then
            Assert.Fail();
        }
    }
}