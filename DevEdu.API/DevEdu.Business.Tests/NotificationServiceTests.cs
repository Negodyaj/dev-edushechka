using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;

namespace DevEdu.Business.Tests
{
    public class NotificationServiceTests
    {
        private Mock<INotificationRepository> _notificationRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;

        private NotificationService _sut;

        [SetUp]
        public void Setup()
        {
            _notificationRepoMock = new Mock<INotificationRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();

            _sut = new NotificationService(
                _notificationRepoMock.Object,
                new NotificationValidationHelper(_notificationRepoMock.Object),
                _groupRepoMock.Object,
                new UserValidationHelper(_userRepoMock.Object),
                new GroupValidationHelper(_groupRepoMock.Object));
        }

        [TestCase(Role.Admin)]
        public void AddNotificationByRole_NotificationDto_NotificationCreated(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoForRole();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(Role.Admin);
            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

            //When
            var actualNotificationtDto = _sut.AddNotification(notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
        }

        [TestCase(Role.Admin)]
        public void AddNotificationByUser_NotificationDto_NotificationCreated(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoForUser();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(Role.Admin);
            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

            //When
            var actualNotificationtDto = _sut.AddNotification(notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
        }

        [TestCase(Role.Admin)]
        public void AddNotificationByGroup_NotificationDto_NotificationCreated(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationForGroupDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(Role.Admin);
            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

            //When
            var actualNotificationtDto = _sut.AddNotification(notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
        }

        [TestCase(Role.Admin)]
        public void GetNotification_ExistingNotificationIdPassed_NotificationReturned(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoForRole();
            const int notificationId = 1;

            _notificationRepoMock.Setup(x => x.GetNotification(notificationId)).Returns(notificationDto);

            //When
            var dto = _sut.GetNotification(notificationId);

            //Than
            Assert.AreEqual(notificationDto, dto);
            _notificationRepoMock.Verify(x => x.GetNotification(notificationId), Times.Once);
        }

        [TestCase(Role.Manager)]
        [TestCase(Role.Admin)]
        public void UpdateNotification_NotificationDto_ReturnUpdatedNotificationDto(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoForRole();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int notificationId = 1;

            _notificationRepoMock.Setup(x => x.UpdateNotification(notificationDto));
            _notificationRepoMock.Setup(x => x.GetNotification(notificationId)).Returns(notificationDto);

            //When
            var dto = _sut.UpdateNotification(notificationId, notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, dto);
            _notificationRepoMock.Verify(x => x.UpdateNotification(notificationDto), Times.Once);
            _notificationRepoMock.Verify(x => x.GetNotification(notificationId), Times.AtLeastOnce);
        }

        [TestCase(Role.Manager)]
        [TestCase(Role.Admin)]
        public void DeleteNotification_ExistingNotificationIdPassed_NotificationRemoved(Enum role)
        {
            //Given
            const int notificationId = 1;
            var notificationDto = NotificationData.GetNotificationDtoForUser();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _notificationRepoMock.Setup(x => x.GetNotification(notificationId)).Returns(notificationDto);
            _notificationRepoMock.Setup(x => x.DeleteNotification(notificationId));


            //When
            _sut.DeleteNotification(notificationId, userInfo);

            //Than
            _notificationRepoMock.Verify(x => x.DeleteNotification(notificationId), Times.Once);
        }

        [Test]
        public void GetNotificationByUserId_IntUserId_ReturnedListOfUserNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByUserDto();
            const int userId = 1;
            var userDto = UserData.GetUserDto();
            _notificationRepoMock.Setup(x => x.GetNotificationsByUserId(userId)).Returns(notificationsList);
            _userRepoMock.Setup(x => x.GetUserById(userId)).Returns(userDto);

            //When
            var listOfDto = _sut.GetNotificationsByUserId(userId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByUserId(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserById(userId), Times.Once);
        }

        [Test]
        public void GetNotificationByGroupId_IntUserId_ReturnedListOfGroupNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByGroupDto();
            const int groupId = 1;
            // var groupDto = GroupData.GetGroupDto();

            _notificationRepoMock.Setup(x => x.GetNotificationsByGroupId(groupId)).Returns(notificationsList);
            // _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(groupDto);

            //When
            var listOfDto = _sut.GetNotificationsByGroupId(groupId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByGroupId(groupId), Times.Once);
            //  _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
        }

        [Test]
        public void GetNotificationByRoleId_IntUserId_ReturnedListOfRoleNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByRoleDto();
            const int userId = 1;

            _notificationRepoMock.Setup(x => x.GetNotificationsByRoleId(userId)).Returns(notificationsList);

            //When
            var listOfDto = _sut.GetNotificationsByRoleId(userId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByRoleId(userId), Times.Once);
        }
    }
}
