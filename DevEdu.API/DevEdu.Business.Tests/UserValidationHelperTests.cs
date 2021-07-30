using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class UserValidationHelperTests
    {
        private Mock<IUserRepository> _userRepoMock;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
        }

        [Test]
        public void CheckUserBelongToGroup_UserDoesntBelongToGroup_ValidationException()
        {
            //Given
            var usersInGroup = UserData.GetListUsersDto();
            var groupId = 0;
            var userId = 4;
            var role = Role.Student;

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)role)).Returns(usersInGroup);

            var sut = new UserValidationHelper(_userRepoMock.Object);

            //When
            Assert.Throws<ValidationException>(() => sut.CheckUserBelongToGroup(groupId, userId, role));

            //Than
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)role), Times.Once);
        }

        [Test]
        public void CheckUserBelongToGroup_GroupDoesntContainsUsers_ValidationException()
        {
            //Given
            List<UserDto> usersInGroup = default;
            var groupId = 0;
            var userId = 2;
            var role = Role.Student;

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)role)).Returns(usersInGroup);

            var sut = new UserValidationHelper(_userRepoMock.Object);

            //When
            Assert.Throws<ValidationException>(() => sut.CheckUserBelongToGroup(groupId, userId, role));

            //Than
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)role), Times.Once);
        }

        [Test]
        public void CheckUserBelongToGroup_UserBelongToGroup_CheckPassed()
        {
            //Given
            var usersInGroup = UserData.GetListUsersDto();
            var groupId = 0;
            var userId = 2;
            var role = Role.Student;

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)role)).Returns(usersInGroup);

            var sut = new UserValidationHelper(_userRepoMock.Object);

            //When
            sut.CheckUserBelongToGroup(groupId, userId, role);

            //Than
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)role), Times.Once);
        }

        [Test]
        public void CheckAuthorizationUserToGroup_UserDoesntBelongToGroup_ValidationException()
        {
            //Given
            var usersInGroup = UserData.GetListUsersDto();
            var groupId = 0;
            var userId = 4;
            var role = Role.Student;

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)role)).Returns(usersInGroup);

            var sut = new UserValidationHelper(_userRepoMock.Object);

            //When
            Assert.Throws<AuthorizationException>(() => sut.CheckAuthorizationUserToGroup(groupId, userId, role));

            //Than
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)role), Times.Once);
        }

        [Test]
        public void CheckAuthorizationUserToGroup_GroupDoesntContainsUsers_ValidationException()
        {
            //Given
            List<UserDto> usersInGroup = default;
            var groupId = 0;
            var userId = 2;
            var role = Role.Student;

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)role)).Returns(usersInGroup);

            var sut = new UserValidationHelper(_userRepoMock.Object);

            //When
            Assert.Throws<AuthorizationException>(() => sut.CheckAuthorizationUserToGroup(groupId, userId, role));

            //Than
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)role), Times.Once);
        }

        [Test]
        public void CheckAuthorizationUserToGroup_UserBelongToGroup_CheckPassed()
        {
            //Given
            var usersInGroup = UserData.GetListUsersDto();
            var groupId = 0;
            var userId = 2;
            var role = Role.Student;

            _userRepoMock.Setup(x => x.GetUsersByGroupIdAndRole(groupId, (int)role)).Returns(usersInGroup);

            var sut = new UserValidationHelper(_userRepoMock.Object);

            //When
            sut.CheckAuthorizationUserToGroup(groupId, userId, role);

            //Than
            _userRepoMock.Verify(x => x.GetUsersByGroupIdAndRole(groupId, (int)role), Times.Once);
        }
    }
}
