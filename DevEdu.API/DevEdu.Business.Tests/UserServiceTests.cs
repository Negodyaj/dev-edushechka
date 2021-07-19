using DevEdu.Business.Services;
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

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
        }

        [Test]
        public void AddUser_DtoWithRoles_UserDtoWithRolesCreated()
        {
            //Given
            var expectedUserId = 1;
            var _userDto = new UserDto
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
            _userRepoMock.Setup(x => x.AddUser(_userDto)).Returns(expectedUserId);
            _userRepoMock.Setup(x => x.AddUserRole(expectedUserId, It.IsAny<int>()));
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualUserId = sut.AddUser(_userDto);

            //Then
            Assert.AreEqual(expectedUserId, actualUserId);
            _userRepoMock.Verify(x => x.AddUser(_userDto), Times.Once);
            _userRepoMock.Verify(x => x.AddUserRole(actualUserId, It.IsAny<int>()), Times.Exactly(_userDto.Roles.Count));
        }

        [Test]
        public void SelectById_IntUserId_ReturnUserDto()
        {
            //Given
            var _userDto = new UserDto
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
            var userId = 1;
            _userRepoMock.Setup(x => x.SelectUserById(userId))
                .Returns(_userDto);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var dto = sut.SelectUserById(userId);

            //Then
            Assert.AreEqual(_userDto, dto);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void SelectUsers_NoEntries_ReturnListUserDto()
        {
            //Given
            var expectedListUserDto = new List<UserDto>
            {
                new UserDto
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
                },
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
                .Returns(expectedListUserDto);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualListUserDto = sut.SelectUsers();

            //Then
            Assert.AreEqual(expectedListUserDto, actualListUserDto);
            _userRepoMock.Verify(x => x.SelectUsers(), Times.Once);
        }

        [Test]
        public void UpdateUser_UserDto_ReturnUpdateUserDto()
        {
            //Given
            var _userDto = new UserDto
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
            _userRepoMock.Setup(x => x.UpdateUser(_userDto));
            _userRepoMock.Setup(x => x.SelectUserById(_userDto.Id)).Returns(expectedUserDto);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualUserDto = sut.UpdateUser(_userDto);

            //Then
            Assert.AreEqual(expectedUserDto, actualUserDto);
            _userRepoMock.Verify(x => x.UpdateUser(_userDto), Times.Once);
        }
    }
}