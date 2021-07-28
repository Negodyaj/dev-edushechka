using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class RatingValidationHelperTests
    {

        private Mock<IRatingRepository> _ratingRepoMock;

        [SetUp]
        public void Setup()
        {
            _ratingRepoMock = new Mock<IRatingRepository>();
        }

        [Test]
        public void CheckRaitingExistence_StudentRatingId_EntityNotFoundException()
        {
            //Given
            StudentRatingDto studentRatingDto = default;

            var studentRatingId = 0;

            _ratingRepoMock.Setup(x => x.SelectStudentRatingById(It.IsAny<int>())).Returns(studentRatingDto);

            var sut = new RatingValidationHelper(_ratingRepoMock.Object);

            //When

            //Than
            Assert.Throws<EntityNotFoundException>(() => sut.CheckRaitingExistence(studentRatingId));
            _ratingRepoMock.Verify(x => x.SelectStudentRatingById(studentRatingId), Times.Exactly(1));
        }
    }
}
