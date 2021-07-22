using DevEdu.Business.Services;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class RaitingServiceTests
    {
        private Mock<IRaitingRepository> _raitingRepoMock;

        [SetUp]
        public void Setup()
        {
            _raitingRepoMock = new Mock<IRaitingRepository>();
        }

        [Test]
        public void AddStudentRaiting_StudentRaitingDto_StudentRaitingCreated()
        {
            //Given
            var studentRaitingDto = RaitingData.GetListOfStudentRaitingDto()[0];

            _raitingRepoMock.Setup(x => x.AddStudentRaiting(studentRaitingDto)).Returns(RaitingData.expectedStudentRaitingId);

            var sut = new RaitingService(_raitingRepoMock.Object);

            //When
            var actualStudentRaitingId = sut.AddStudentRaiting(studentRaitingDto);

            //Than
            Assert.AreEqual(RaitingData.expectedStudentRaitingId, actualStudentRaitingId);
            _raitingRepoMock.Verify(x => x.AddStudentRaiting(studentRaitingDto), Times.Once);
        }

        [Test]
        public void GetAllStudentRaitings_NoEntries_ReturnListOfStudentRaitingDto()
        {
            //Given
            var expectedStudentRaitingDtos = RaitingData.GetListOfStudentRaitingDto();

            _raitingRepoMock.Setup(x => x.SelectAllStudentRaitings()).Returns(expectedStudentRaitingDtos);

            var sut = new RaitingService(_raitingRepoMock.Object);

            //When
            var actualStudentRaitingDtos = sut.GetAllStudentRaitings();

            //Than
            Assert.AreEqual(expectedStudentRaitingDtos, actualStudentRaitingDtos);
            _raitingRepoMock.Verify(x => x.SelectAllStudentRaitings(), Times.Once);
        }

        [Test]
        public void GetStudentRaitingById_Id_ReturnStudentRaitingDto()
        {
            //Given
            var expectedStudentRaitingDto = RaitingData.GetListOfStudentRaitingDto()[0];

            _raitingRepoMock.Setup(x => x.SelectStudentRaitingById(RaitingData.expectedStudentRaitingId)).Returns(expectedStudentRaitingDto);

            var sut = new RaitingService(_raitingRepoMock.Object);

            //When
            var actualStudentRaitingDto = sut.GetStudentRaitingById(RaitingData.expectedStudentRaitingId);

            //Than
            Assert.AreEqual(expectedStudentRaitingDto, actualStudentRaitingDto);
            _raitingRepoMock.Verify(x => x.SelectStudentRaitingById(RaitingData.expectedStudentRaitingId), Times.Once);
        }

        [Test]
        public void GetStudentRaitingByUserId_UserId_ReturnListOfStudentRaitingDto()
        {
            //Given
            var expectedStudentRaitingDtos = RaitingData.GetListOfStudentRaitingDto();

            _raitingRepoMock.Setup(x => x.SelectStudentRaitingByUserId(UserData.expectedUserId)).Returns(expectedStudentRaitingDtos);

            var sut = new RaitingService(_raitingRepoMock.Object);

            //When
            var actualStudentRaitingDtos = sut.GetStudentRaitingByUserId(UserData.expectedUserId);

            //Than
            Assert.AreEqual(expectedStudentRaitingDtos, actualStudentRaitingDtos);
            _raitingRepoMock.Verify(x => x.SelectStudentRaitingByUserId(UserData.expectedUserId), Times.Once);
        }

        [Test]
        public void GetStudentRaitingByGroupId_GroupId_ReturnListOfStudentRaitingDto()
        {
            //Given
            var expectedStudentRaitingDtos = RaitingData.GetListOfStudentRaitingDto();
            var groupId = expectedStudentRaitingDtos[0].Group.Id;

            _raitingRepoMock.Setup(x => x.SelectStudentRaitingByGroupId(groupId)).Returns(expectedStudentRaitingDtos);

            var sut = new RaitingService(_raitingRepoMock.Object);

            //When
            var actualStudentRaitingDtos = sut.GetStudentRaitingByGroupId(groupId);

            //Than
            Assert.AreEqual(expectedStudentRaitingDtos, actualStudentRaitingDtos);
            _raitingRepoMock.Verify(x => x.SelectStudentRaitingByGroupId(groupId), Times.Once);
        }

        [Test]
        public void UpdateStudentRaiting_Id_Value_PeriodNumber_ReturnStudentRaitingDto()
        {
            //Given
            var expectedStudentRaitingDto = RaitingData.GetListOfStudentRaitingDto()[0];
            var id = expectedStudentRaitingDto.Id;
            var value = expectedStudentRaitingDto.Raiting;
            var periodNumber = expectedStudentRaitingDto.ReportingPeriodNumber;

            _raitingRepoMock.Setup(x => x.UpdateStudentRaiting(RaitingData.GetListOfStudentRaitingDto()[2]));
            _raitingRepoMock.Setup(x => x.SelectStudentRaitingById(RaitingData.expectedStudentRaitingId))
                .Returns(expectedStudentRaitingDto);

            var sut = new RaitingService(_raitingRepoMock.Object);

            //When
            var actualStudentRaitingDto = sut.UpdateStudentRaiting(id, value, periodNumber);

            //Than
            Assert.AreEqual(expectedStudentRaitingDto, actualStudentRaitingDto);
            _raitingRepoMock.Verify(x => x.UpdateStudentRaiting(It.Is<StudentRaitingDto>(dto => 
                dto.Equals(RaitingData.GetListOfStudentRaitingDto()[2]))));
            _raitingRepoMock.Verify(x => x.SelectStudentRaitingById(id), Times.Once);
        }
    }
}
