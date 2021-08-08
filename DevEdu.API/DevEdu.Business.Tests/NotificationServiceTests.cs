using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class NotificationServiceTests
    {
        private Mock<INotificationRepository> _notificationRepoMock;

        [SetUp]
        public void Setup()
        {
            _notificationRepoMock = new Mock<INotificationRepository>();
        }

        [Test]
        public void AddNotificationByRole_NotificationDto_NotificationCreated()
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoByRole();

            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var actualNotificationtDto = sut.AddNotification(notificationDto);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
        }
        [Test]
        public void AddNotificationByUser_NotificationDto_NotificationCreated()
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoByUser();

            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var actualNotificationtDto = sut.AddNotification(notificationDto);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
        }
        [Test]
        public void AddNotificationByGroup_NotificationDto_NotificationCreated()
        {
            //Given
            var notificationDto = NotificationData.GetNotificationByGroupDto();

            var ExpectedNotificationId = 1;

            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var actualNotificationtDto = sut.AddNotification(notificationDto);

            //Than
            Assert.AreEqual(notificationDto, actualNotificationtDto);
            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
        }

        [Test]
        public void GetNotification_NotificationDto_GetNotification()
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoByRole();
            const int notificationId = 1;

            _notificationRepoMock.Setup(x => x.GetNotification(notificationId)).Returns(notificationDto);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var dto = sut.GetNotification(notificationId);

            //Than
            Assert.AreEqual(notificationDto, dto);
            _notificationRepoMock.Verify(x => x.GetNotification(notificationId), Times.Once);
        }

        [Test]
        public void UpdateNotification_NotificationDto_ReturnUpdatedNotificationDto()
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDtoByRole();
            const int notificationId = 1;

            _notificationRepoMock.Setup(x => x.UpdateNotification(notificationDto));
            _notificationRepoMock.Setup(x => x.GetNotification(notificationId)).Returns(notificationDto);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var dto = sut.UpdateNotification(notificationId, notificationDto);

            //Than
            Assert.AreEqual(notificationDto, dto);
            _notificationRepoMock.Verify(x => x.UpdateNotification(notificationDto), Times.Once);
            _notificationRepoMock.Verify(x => x.GetNotification(notificationId), Times.Once);
        }

        [Test]
        public void DeleteNotification_IntNotificationId_DeleteNotification()
        {
            //Given
            const int notificationId = 1;

            _notificationRepoMock.Setup(x => x.DeleteNotification(notificationId));

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            sut.DeleteNotification(notificationId);

            //Than
            _notificationRepoMock.Verify(x => x.DeleteNotification(notificationId), Times.Once);
        }

        [Test]
        public void GetNotificationByUserId_IntUserId_ReturnedListOfUserNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByUserDto();
            const int userId = 1;

            _notificationRepoMock.Setup(x => x.GetNotificationsByUserId(userId)).Returns(notificationsList);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var listOfDto = sut.GetNotificationsByUserId(userId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByUserId(userId), Times.Once);
        }

        [Test]
        public void GetNotificationByGroupId_IntUserId_ReturnedListOfGroupNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByGroupDto();
            const int userId = 1;

            _notificationRepoMock.Setup(x => x.GetNotificationsByGroupId(userId)).Returns(notificationsList);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var listOfDto = sut.GetNotificationsByGroupId(userId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByGroupId(userId), Times.Once);
        }

        [Test]
        public void GetNotificationByRoleId_IntUserId_ReturnedListOfRoleNotifications()
        {
            //Given
            var notificationsList = NotificationData.GetListNotificationByRoleDto();
            const int userId = 1;

            _notificationRepoMock.Setup(x => x.GetNotificationsByRoleId(userId)).Returns(notificationsList);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var listOfDto = sut.GetNotificationsByRoleId(userId);

            //Than
            Assert.AreEqual(notificationsList, listOfDto);
            _notificationRepoMock.Verify(x => x.GetNotificationsByRoleId(userId), Times.Once);
        }
    }
}
