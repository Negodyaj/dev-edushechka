using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class RatingServiceTests
    {
        private Mock<IRatingRepository> _ratingRepoMock;
        private Mock<IRatingValidationHelper> _ratingValidationHelperMock;
        private Mock<IUserValidationHelper> _userValidationHelperMock;
        private Mock<IGroupValidationHelper> _groupValidationHelperMock;

        [SetUp]
        public void Setup()
        {
            _ratingRepoMock = new Mock<IRatingRepository>();
            _ratingValidationHelperMock = new Mock<IRatingValidationHelper>();
            _userValidationHelperMock = new Mock<IUserValidationHelper>();
            _groupValidationHelperMock = new Mock<IGroupValidationHelper>();
        }

        [Test]
        public void AddStudentRating_StudentRatingDto_AuthorUserId_StudentRatingCreated()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetListOfStudentRatingDto()[0];
            var studentRatingId = expectedStudentRatingDto.Id;
            var userId = expectedStudentRatingDto.User.Id;
            var authorUserId = 1;
            var groupId = expectedStudentRatingDto.Group.Id;

            _ratingRepoMock.Setup(x => x.AddStudentRating(expectedStudentRatingDto)).Returns(studentRatingId);
            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingDto = sut.AddStudentRating(expectedStudentRatingDto, authorUserId);

            //Than
            Assert.AreEqual(expectedStudentRatingDto, actualStudentRatingDto);
            _ratingRepoMock.Verify(x => x.AddStudentRating(expectedStudentRatingDto), Times.Once);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _groupValidationHelperMock.Verify(x => x.CheckGroupExistence(groupId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckAuthorizationUserToGroup(groupId, authorUserId, Role.Teacher),
                Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(userId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserBelongToGroup(groupId, userId, Role.Student), Times.Once);
        }

        [Test]
        public void DeleteStudentRating_StudentRatingId_AuthorUserId_StudentRatingDeleted()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetListOfStudentRatingDto()[0];
            var studentRatingId = expectedStudentRatingDto.Id;
            var authorUserId = 1;
            var groupId = expectedStudentRatingDto.Group.Id;

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            sut.DeleteStudentRating(studentRatingId, authorUserId);

            //Than
            _userValidationHelperMock.Verify(x => x.CheckAuthorizationUserToGroup(groupId, authorUserId, Role.Teacher),
                Times.Once);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _ratingRepoMock.Verify(x => x.DeleteStudentRating(studentRatingId), Times.Once);
        }

        [Test]
        public void DeleteStudentRating_EntityDoesntExist_EntityNotFoundException()
        {
            //Given
            StudentRatingDto expectedStudentRatingDto = default;
            var studentRatingId = 1;
            var authorUserId = 1;

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId)).Returns(expectedStudentRatingDto);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            Assert.Throws<EntityNotFoundException>(() => sut.DeleteStudentRating(studentRatingId, authorUserId));

            //Than
            _userValidationHelperMock.Verify(x => x.CheckAuthorizationUserToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Role>()),
                Times.Never);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
            _ratingRepoMock.Verify(x => x.DeleteStudentRating(It.IsAny<int>()), Times.Never);
        }

        [Test]
        public void GetAllStudentRatings_NoEntries_ReturnListOfStudentRatingDto()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();

            _ratingRepoMock.Setup(x => x.SelectAllStudentRatings()).Returns(expectedStudentRatingDtos);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingDtos = sut.GetAllStudentRatings();

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectAllStudentRatings(), Times.Once);
        }

        [Test]
        public void GetStudentRatingByUserId_UserId_ReturnListOfStudentRatingDto()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();

            _ratingRepoMock.Setup(x => x.SelectStudentRatingByUserId(UserData.expectedUserId)).Returns(expectedStudentRatingDtos);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingDtos = sut.GetStudentRatingByUserId(UserData.expectedUserId);

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByUserId(UserData.expectedUserId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckUserExistence(UserData.expectedUserId), Times.Once);
        }

        [Test]
        public void GetStudentRatingByGroupId_GroupId_AuthorUserId_RoleManager_ReturnListOfStudentRatingDto()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var groupId = expectedStudentRatingDtos[0].Group.Id;
            var authorUserId = 1;
            var authorUserRoles = new List<Role> { Role.Manager };

            _ratingRepoMock.Setup(x => x.SelectStudentRatingByGroupId(groupId)).Returns(expectedStudentRatingDtos);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingDtos = sut.GetStudentRatingByGroupId(groupId, authorUserId, authorUserRoles);

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByGroupId(groupId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckAuthorizationUserToGroup(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Role>()), Times.Never);
        }

        [Test]
        public void GetStudentRatingByGroupId_GroupId_AuthorUserId_RoleTeacher_ReturnListOfStudentRatingDto()
        {
            //Given
            var expectedStudentRatingDtos = RatingData.GetListOfStudentRatingDto();
            var groupId = expectedStudentRatingDtos[0].Group.Id;
            var authorUserId = 1;
            var authorUserRoles = new List<Role> { Role.Teacher };

            _ratingRepoMock.Setup(x => x.SelectStudentRatingByGroupId(groupId)).Returns(expectedStudentRatingDtos);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingDtos = sut.GetStudentRatingByGroupId(groupId, authorUserId, authorUserRoles);

            //Than
            Assert.AreEqual(expectedStudentRatingDtos, actualStudentRatingDtos);
            _ratingRepoMock.Verify(x => x.SelectStudentRatingByGroupId(groupId), Times.Once);
            _userValidationHelperMock.Verify(x => x.CheckAuthorizationUserToGroup(groupId, authorUserId, Role.Teacher), Times.Once);
        }

        [Test]
        public void UpdateStudentRating_Id_Value_PeriodNumber_ReturnStudentRatingDto()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetListOfStudentRatingDto()[0];
            var studentRatingId = expectedStudentRatingDto.Id;
            var groupId = expectedStudentRatingDto.Group.Id;
            var value = expectedStudentRatingDto.Rating;
            var periodNumber = expectedStudentRatingDto.ReportingPeriodNumber;
            var authorUserId = 1;

            _ratingRepoMock.Setup(x => x.UpdateStudentRating(RatingData.GetListOfStudentRatingDto()[2]));
            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(studentRatingId))
                .Returns(expectedStudentRatingDto);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingDto = sut.UpdateStudentRating(studentRatingId, value, periodNumber, authorUserId);

            //Than
            Assert.AreEqual(expectedStudentRatingDto, actualStudentRatingDto);
            _ratingRepoMock.Verify(x => x.UpdateStudentRating(It.Is<StudentRatingDto>(dto =>
                dto.Equals(RatingData.GetListOfStudentRatingDto()[2]))));
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Exactly(2));
            _userValidationHelperMock.Verify(x => x.CheckAuthorizationUserToGroup(groupId, authorUserId, Role.Teacher), Times.Once);
        }

        [Test]
        public void UpdateStudentRating_Id_Value_PeriodNumber_EntityNotFoundException()
        {
            //Given
            StudentRatingDto expectedStudentRatingDto = default;
            var studentRatingId = 0;
            var value = 0;
            var periodNumber = 0;
            var authorUserId = 0;

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(It.IsAny<int>())).Returns(expectedStudentRatingDto);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When

            //Than
            Assert.Throws<EntityNotFoundException>(() => sut.UpdateStudentRating(studentRatingId, value, periodNumber, authorUserId));
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Once);
        }
    }
}
