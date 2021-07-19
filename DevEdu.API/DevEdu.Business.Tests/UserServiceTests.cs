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

        [TestCaseSource(typeof(UserServiceTestsSource))]
        public void AddUser_DtoWithRoles_UserDtoWithRolesCreated(UserDto user)
        {
            //Given
            var expectedId = 1;
            _userRepoMock.Setup(x => x.AddUser(user)).Returns(expectedId);
            _userRepoMock.Setup(x => x.AddUserRole(expectedId, It.IsAny<int>()));
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualId = sut.AddUser(user);

            //Then
            Assert.AreEqual(expectedId, actualId);
            _userRepoMock.Verify(x => x.AddUser(user), Times.Once);
            _userRepoMock.Verify(x => x.AddUserRole(actualId, It.IsAny<int>()), Times.Exactly(user.Roles.Count));
        }

        [TestCaseSource(typeof(UserServiceTestsSource))]
        public void SelectById_IntUserId_ReturnUserDto(UserDto expectedDto)
        {
            //Given
            var id = 1;
            _userRepoMock.Setup(x => x.SelectUserById(id)).Returns(expectedDto);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualDto = sut.SelectUserById(id);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _userRepoMock.Verify(x => x.SelectUserById(id), Times.Once);
        }

        [TestCaseSource(typeof(UserServiceTestsSource), nameof(UserServiceTestsSource.GetUsers))]
        public void SelectUsers_NoEntries_ReturnListUserDto(List<UserDto> expectedList)
        {
            //Given
            _userRepoMock.Setup(x => x.SelectUsers()).Returns(expectedList);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualList = sut.SelectUsers();

            //Then
            Assert.AreEqual(expectedList, actualList);
            _userRepoMock.Verify(x => x.SelectUsers(), Times.Once);
        }

        [TestCaseSource(typeof(UserServiceTestsSource))]
        public void UpdateUser_UserDto_ReturnUpdateUserDto(UserDto expectedDto)
        {
            //Given
            _userRepoMock.Setup(x => x.UpdateUser(expectedDto));
            _userRepoMock.Setup(x => x.SelectUserById(expectedDto.Id)).Returns(expectedDto);
            var sut = new UserService(_userRepoMock.Object);

            //When
            var actualDto = sut.UpdateUser(expectedDto);

            //Then
            Assert.AreEqual(expectedDto, actualDto);
            _userRepoMock.Verify(x => x.UpdateUser(expectedDto), Times.Once);
        }
    }
}