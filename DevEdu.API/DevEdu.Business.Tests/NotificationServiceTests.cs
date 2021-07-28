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
        public void AddNotification_NotificationDto_NotificationCreated()
        {
            //Given
            var notificationDto = NotificationData.GetNotificationDto();

            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(NotificationData.ExpectedNotificationId);

            var sut = new NotificationService(_notificationRepoMock.Object);

            //When
            var actualNotificationtId = sut.AddNotification(notificationDto);

            //Than
            Assert.AreEqual(NotificationData.ExpectedNotificationId, actualNotificationtId);
            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
        }
    }
}
