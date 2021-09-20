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
            _sut = new NotificationService(_notificationRepoMock.Object,
                new NotificationValidationHelper(_notificationRepoMock.Object),
                _groupRepoMock.Object,
                new UserValidationHelper(_userRepoMock.Object),
                new GroupValidationHelper(_groupRepoMock.Object));

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

            _notificationRepoMock.Setup(x => x.AddNotificationAsync(notificationDto)).ReturnsAsync(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotificationAsync(ExpectedNotificationId)).ReturnsAsync(notificationDto);

            //When
            var actualNotificationtDto = _sut.AddNotificationAsync(notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotificationAsync(notificationDto), Times.Once);
        }

        [TestCase(Role.Admin)]
        public void AddNotificationByUser_NotificationDto_NotificationCreated(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoForUser();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(Role.Admin);
            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotificationAsync(notificationDto)).ReturnsAsync(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotificationAsync(ExpectedNotificationId)).ReturnsAsync(notificationDto);

            //When
            var actualNotificationtDto = _sut.AddNotificationAsync(notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotificationAsync(notificationDto), Times.Once);
        }

        [TestCase(Role.Admin)]
        public void AddNotificationByGroup_NotificationDto_NotificationCreated(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationForGroupDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(Role.Admin);
            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotificationAsync(notificationDto)).ReturnsAsync(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotificationAsync(ExpectedNotificationId)).ReturnsAsync(notificationDto);

            //When
            var actualNotificationtDto = _sut.AddNotificationAsync(notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotificationAsync(notificationDto), Times.Once);
        }

        [TestCase(Role.Admin)]
        public void GetNotification_ExistingNotificationIdPassed_NotificationReturned(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoForRole();
            const int notificationId = 1;

            _notificationRepoMock.Setup(x => x.GetNotificationAsync(notificationId)).ReturnsAsync(notificationDto);

            //When
            var dto = _sut.GetNotificationAsync(notificationId);

            //Than
            Assert.AreEqual(notificationDto, dto);
            _notificationRepoMock.Verify(x => x.GetNotificationAsync(notificationId), Times.Once);
        }

        [TestCase(Role.Manager)]
        [TestCase(Role.Admin)]
        public void UpdateNotification_NotificationDto_ReturnUpdatedNotificationDto(Enum role)
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoForRole();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            const int notificationId = 1;

            _notificationRepoMock.Setup(x => x.UpdateNotificationAsync(notificationDto));
            _notificationRepoMock.Setup(x => x.GetNotificationAsync(notificationId)).ReturnsAsync(notificationDto);

            //When
            var dto = _sut.UpdateNotificationAsync(notificationId, notificationDto, userInfo);

            //Than
            Assert.AreEqual(notificationDto, dto);
            _notificationRepoMock.Verify(x => x.UpdateNotificationAsync(notificationDto), Times.Once);
            _notificationRepoMock.Verify(x => x.GetNotificationAsync(notificationId), Times.AtLeastOnce);
        }

        [TestCase(Role.Manager)]
        [TestCase(Role.Admin)]
        public async System.Threading.Tasks.Task DeleteNotification_ExistingNotificationIdPassed_NotificationRemovedAsync(Enum role)
        {
            //Given
            const int notificationId = 1;
            var notificationDto = NotificationData.GetNotificationDtoForUser();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

            _notificationRepoMock.Setup(x => x.GetNotificationAsync(notificationId)).ReturnsAsync(notificationDto);
            _notificationRepoMock.Setup(x => x.DeleteNotificationAsync(notificationId));

            //When
           await _sut.DeleteNotificationAsync(notificationId, userInfo);

            //Than
            _notificationRepoMock.Verify(x => x.DeleteNotificationAsync(notificationId), Times.Once);
        }

        [Test]
        public void GetNotificationByUserId_IntUserId_ReturnedListOfUserNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByUserDto();
            const int userId = 1;
            var userDto = UserData.GetUserDto();
            _notificationRepoMock.Setup(x => x.GetNotificationsByUserIdAsync(userId)).ReturnsAsync(notificationsList);
            _userRepoMock.Setup(x => x.GetUserByIdAsync(userId)).ReturnsAsync(userDto);

            //When
            var listOfDto = _sut.GetNotificationsByUserIdAsync(userId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByUserIdAsync(userId), Times.Once);
            _userRepoMock.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }

        [Test]
        public void GetNotificationByGroupId_IntUserId_ReturnedListOfGroupNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByGroupDto();
            const int groupId = 1;
            // var groupDto = GroupData.GetGroupDto();

            _notificationRepoMock.Setup(x => x.GetNotificationsByGroupIdAsync(groupId)).ReturnsAsync(notificationsList);
            // _groupRepoMock.Setup(x => x.GetGroup(groupId)).Returns(groupDto);

            //When
            var listOfDto = _sut.GetNotificationsByGroupIdAsync(groupId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByGroupIdAsync(groupId), Times.Once);
            //  _groupRepoMock.Verify(x => x.GetGroup(groupId), Times.Once);
        }

        [Test]
        public void GetNotificationByRoleId_IntUserId_ReturnedListOfRoleNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByRoleDto();
            const int userId = 1;

            _notificationRepoMock.Setup(x => x.GetNotificationsByRoleIdAsync(userId)).ReturnsAsync(notificationsList);

            //When
            var listOfDto = _sut.GetNotificationsByRoleIdAsync(userId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByRoleIdAsync(userId), Times.Once);
        }
    }
}
