//using DevEdu.Business.Services;
//using DevEdu.Business.ValidationHelpers;
//using DevEdu.DAL.Enums;
//using DevEdu.DAL.Repositories;
//using Moq;
//using NUnit.Framework;
//using System;

//namespace DevEdu.Business.Tests
//{
//    public class NotificationServiceTests
//    {
//        private Mock<INotificationRepository> _notificationRepoMock;
//        private Mock<IUserRepository> _userRepoMock;
//        private NotificationValidationHelper _notificationValidationHelper;
//        private UserValidationHelper _userValidationHelper;

//        [SetUp]
//        public void Setup()
//        {
//            _notificationRepoMock = new Mock<INotificationRepository>();
//            _userRepoMock = new Mock<IUserRepository>();
//            _userValidationHelper = new UserValidationHelper(_userRepoMock.Object);
//            _notificationValidationHelper = new NotificationValidationHelper(_notificationRepoMock.Object, _userValidationHelper);
//        }

//        [Test]
//        public void AddNotificationByRole_NotificationDto_NotificationCreated(Enum role)
//        {
//            //Given
//            var notificationDto = NotificationData.GetNotificationDtoByRole();

//            var ExpectedNotificationId = 1;

//            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
//            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var actualNotificationtDto = sut.AddNotification(notificationDto);

//            //Than
//            Assert.AreEqual(notificationDto, actualNotificationtDto);
//            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
//        }
//        [Test]
//        public void AddNotificationByUser_NotificationDto_NotificationCreated(Enum role)
//        {
//            //Given
//            var notificationDto = NotificationData.GetNotificationDtoByUser();

//            var ExpectedNotificationId = 1;

//            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
//            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var actualNotificationtDto = sut.AddNotification(notificationDto);

//            //Than
//            Assert.AreEqual(notificationDto, actualNotificationtDto);
//            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
//        }
//        [Test]
//        public void AddNotificationByGroup_NotificationDto_NotificationCreated(Enum role)
//        {
//            //Given
//            var notificationDto = NotificationData.GetNotificationByGroupDto();

//            var ExpectedNotificationId = 1;

//            _notificationRepoMock.Setup(x => x.AddNotification(notificationDto)).Returns(ExpectedNotificationId);
//            _notificationRepoMock.Setup(x => x.GetNotification(ExpectedNotificationId)).Returns(notificationDto);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var actualNotificationtDto = sut.AddNotification(notificationDto);

//            //Than
//            Assert.AreEqual(notificationDto, actualNotificationtDto);
//            _notificationRepoMock.Verify(x => x.AddNotification(notificationDto), Times.Once);
//        }

//        [Test]
//        public void GetNotification_NotificationDto_GetNotification(Enum role)
//        {
//            //Given
//            var notificationDto = NotificationData.GetNotificationDtoByRole();
//            const int notificationId = 1;

//            _notificationRepoMock.Setup(x => x.GetNotification(notificationId)).Returns(notificationDto);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var dto = sut.GetNotification(notificationId);

//            //Than
//            Assert.AreEqual(notificationDto, dto);
//            _notificationRepoMock.Verify(x => x.GetNotification(notificationId), Times.Once);
//        }

//        [TestCase(Role.Teacher)]
//        [TestCase(Role.Manager)]
//        public void UpdateNotification_NotificationDto_ReturnUpdatedNotificationDto(Enum role)
//        {
//            //Given
//            var notificationDto = NotificationData.GetNotificationDtoByRole();
//            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
//            const int notificationId = 1;

//            _notificationRepoMock.Setup(x => x.UpdateNotification(notificationDto));
//            _notificationRepoMock.Setup(x => x.GetNotification(notificationId)).Returns(notificationDto);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var dto = sut.UpdateNotification(notificationId, notificationDto, userInfo);

//            //Than
//            Assert.AreEqual(notificationDto, dto);
//            _notificationRepoMock.Verify(x => x.UpdateNotification(notificationDto), Times.Once);
//            _notificationRepoMock.Verify(x => x.GetNotification(notificationId), Times.Once);
//        }

//        [TestCase(Role.Teacher)]
//        [TestCase(Role.Manager)]
//        public void DeleteNotification_IntNotificationId_DeleteNotification(Enum role)
//        {
//            //Given
//            const int notificationId = 1;
//            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);

//            _notificationRepoMock.Setup(x => x.DeleteNotification(notificationId));

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            sut.DeleteNotification(notificationId, userInfo);

//            //Than
//            _notificationRepoMock.Verify(x => x.DeleteNotification(notificationId), Times.Once);
//        }

//        [Test]
//        public void GetNotificationByUserId_IntUserId_ReturnedListOfUserNotifications(Enum role)
//        {
//            //Given
//            var notificationsList = NotificationData.GetListNotificationByUserDto();
//            const int userId = 1;

//            _notificationRepoMock.Setup(x => x.GetNotificationsByUserId(userId)).Returns(notificationsList);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var listOfDto = sut.GetNotificationsByUserId(userId);

//            //Than
//            Assert.AreEqual(notificationsList, listOfDto);
//            _notificationRepoMock.Verify(x => x.GetNotificationsByUserId(userId), Times.Once);
//        }

//        [Test]
//        public void GetNotificationByGroupId_IntUserId_ReturnedListOfGroupNotifications(Enum role)
//        {
//            //Given
//            var notificationsList = NotificationData.GetListNotificationByGroupDto();
//            const int userId = 1;

//            _notificationRepoMock.Setup(x => x.GetNotificationsByGroupId(userId)).Returns(notificationsList);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var listOfDto = sut.GetNotificationsByGroupId(userId);

//            //Than
//            Assert.AreEqual(notificationsList, listOfDto);
//            _notificationRepoMock.Verify(x => x.GetNotificationsByGroupId(userId), Times.Once);
//        }

//        [Test]
//        public void GetNotificationByRoleId_IntUserId_ReturnedListOfRoleNotifications(Enum role)
//        {
//            //Given
//            var notificationsList = NotificationData.GetListNotificationByRoleDto();
//            const int userId = 1;

//            _notificationRepoMock.Setup(x => x.GetNotificationsByRoleId(userId)).Returns(notificationsList);

//            var sut = new NotificationService(_notificationRepoMock.Object, _notificationValidationHelper);

//            //When
//            var listOfDto = sut.GetNotificationsByRoleId(userId);

//            //Than
//            Assert.AreEqual(notificationsList, listOfDto);
//            _notificationRepoMock.Verify(x => x.GetNotificationsByRoleId(userId), Times.Once);
//        }
//    }
//}
