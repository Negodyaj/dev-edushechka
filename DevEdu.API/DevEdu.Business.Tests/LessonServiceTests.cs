using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class LessonServiceTests
    {
        private Mock<ILessonRepository> _lessonRepoMock;
        private Mock<ICommentRepository> _commentRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private UserValidationHelper _userValidationHelper;
        private LessonValidationHelper _lessonValidationHelper;
        private LessonService _sut;

        [SetUp]
        public void Setup()
        {
            _lessonRepoMock = new Mock<ILessonRepository>();
            _commentRepoMock = new Mock<ICommentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _lessonValidationHelper = new LessonValidationHelper(_lessonRepoMock.Object);
            _userValidationHelper = new UserValidationHelper(_userRepoMock.Object);


            _sut = new LessonService(
                    _lessonRepoMock.Object,
                    _commentRepoMock.Object,
                    _userRepoMock.Object,
                    _userValidationHelper,
                    _lessonValidationHelper);
        }

        [Test]
        public void AddStudentToLesson_IntLessonIdAndUserId_AddingStudentToLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.AddStudentToLesson(lessonId, userId));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);            

            //When
            var dto = _sut.AddStudentToLesson(lessonId, userId);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.AddStudentToLesson(lessonId, userId), Times.Once);           
        }

        [Test]
        public void DeleteStudentFromLesson_IntLessonIdAndUserId_DeleteStudentFromLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.DeleteStudentFromLesson(lessonId, userId));


            //When
            _sut.DeleteStudentFromLesson(lessonId,userId);

            //Than
            _lessonRepoMock.Verify(x => x.DeleteStudentFromLesson(lessonId, userId), Times.Once);
        }


        [Test]
        public void UpdateFeedback_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.UpdateStudentFeedbackForLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(LessonData.GetLessonDto);
            _userRepoMock.Setup(x => x.SelectUserById(userId)).Returns(LessonData.GetUserDto);
                       
            //When
            var dto = _sut.UpdateStudentFeedbackForLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentFeedbackForLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _userRepoMock.Verify(x => x.SelectUserById(userId), Times.Once);
        }

        [Test]
        public void UpdateAbsenceReason_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            //When
            var dto = _sut.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateAttendance_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = 30;
            var userId = 42;

            _lessonRepoMock.Setup(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);


            //When
            var dto = _sut.UpdateStudentAttendanceOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectAttendanceByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void GetAllFeedback_IntLessonId_ReturnedListStuentLessenDto()
        {
            //Given
            var lessonId = 30;
            var listStudentLessonDto = LessonData.GetListStudentDto();

            _lessonRepoMock.Setup(x => x.SelectAllFeedbackByLessonId(lessonId)).Returns(listStudentLessonDto);
             
            //When
            var listOfDto = _sut.SelectAllFeedbackByLessonId(lessonId);

            //Than
            Assert.AreEqual(listStudentLessonDto, listOfDto);
            _lessonRepoMock.Verify(x => x.SelectAllFeedbackByLessonId(lessonId), Times.Once);
        }
    }
}