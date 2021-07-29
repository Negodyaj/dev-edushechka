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
        public void AddStudentRating_StudentRatingDto_StudentRatingCreated()
        {
            //Given
            var studentRatingDto = RatingData.GetListOfStudentRatingDto()[0];

            var expectedStudentRatingId = studentRatingDto.Id;

            var authorUserId = 1;

            _ratingRepoMock.Setup(x => x.AddStudentRating(studentRatingDto)).Returns(expectedStudentRatingId);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingId = sut.AddStudentRating(studentRatingDto, authorUserId);

            //Than
            Assert.AreEqual(expectedStudentRatingId, actualStudentRatingId);
            _ratingRepoMock.Verify(x => x.AddStudentRating(studentRatingDto), Times.Once);
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
        }

        [Test]
        public void GetStudentRatingByGroupId_GroupId_ReturnListOfStudentRatingDto()
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
        }

        [Test]
        public void UpdateStudentRating_Id_Value_PeriodNumber_ReturnStudentRatingDto()
        {
            //Given
            var expectedStudentRatingDto = RatingData.GetListOfStudentRatingDto()[0];
            var StudentRatingId = expectedStudentRatingDto.Id;
            var value = expectedStudentRatingDto.Rating;
            var periodNumber = expectedStudentRatingDto.ReportingPeriodNumber;
            var authorUserId = 1;

            _ratingRepoMock.Setup(x => x.UpdateStudentRating(RatingData.GetListOfStudentRatingDto()[2]));
            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(StudentRatingId))
                .Returns(expectedStudentRatingDto);

            var sut = new RatingService(_ratingRepoMock.Object, _ratingValidationHelperMock.Object, _groupValidationHelperMock.Object,
                _userValidationHelperMock.Object);

            //When
            var actualStudentRatingDto = sut.UpdateStudentRating(StudentRatingId, value, periodNumber, authorUserId);

            //Than
            Assert.AreEqual(expectedStudentRatingDto, actualStudentRatingDto);
            _ratingRepoMock.Verify(x => x.UpdateStudentRating(It.Is<StudentRatingDto>(dto =>
                dto.Equals(RatingData.GetListOfStudentRatingDto()[2]))));
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(StudentRatingId), Times.Exactly(2));
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
