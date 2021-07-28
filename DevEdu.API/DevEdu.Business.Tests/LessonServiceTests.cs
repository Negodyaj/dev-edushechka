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
        private Mock<IUserValidationHelper> _userValidationHelperMock;
        private Mock<ILessonValidationHelper> _lessonValidationHelperMock;


        [SetUp]
        public void Setup()
        {
            _lessonRepoMock = new Mock<ILessonRepository>();
            _commentRepoMock = new Mock<ICommentRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _userValidationHelperMock = new Mock<IUserValidationHelper>();
            _lessonValidationHelperMock = new Mock<ILessonValidationHelper>();
        }

        [Test]
        public void AddStudentToLesson_IntLessonIdAndUserId_AddingStudentToLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            
            var lessonId = LessonData.LessonId;
            var userId = LessonData.UserId;

            _lessonRepoMock.Setup(x => x.AddStudentToLesson(lessonId, userId));
            _lessonRepoMock.Setup(x => x.SelectByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var dto = sut.AddStudentToLesson(lessonId, userId);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.AddStudentToLesson(lessonId, userId), Times.Once);           
        }

        [Test]
        public void DeleteStudentFromLesson_IntLessonIdAndUserId_DeleteStudentFromLesson()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = LessonData.LessonId;
            var userId = LessonData.UserId;

            _lessonRepoMock.Setup(x => x.DeleteStudentFromLesson(lessonId, userId));

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            sut.DeleteStudentFromLesson(lessonId,userId);

            //Than
            _lessonRepoMock.Verify(x => x.DeleteStudentFromLesson(lessonId, userId), Times.Once);
        }


        [Test]
        public void UpdateFeedback_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = LessonData.LessonId;
            var userId = LessonData.UserId;

            _lessonRepoMock.Setup(x => x.UpdateStudentFeedbackForLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var dto = sut.UpdateStudentFeedbackForLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentFeedbackForLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateAbsenceReason_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = LessonData.LessonId;
            var userId = LessonData.UserId;

            _lessonRepoMock.Setup(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var dto = sut.UpdateStudentAbsenceReasonOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentAbsenceReasonOnLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void UpdateAttendance_IntLessonIdUserIdAndStuentLessonDto_ReturnUpdatedStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = LessonData.LessonId;
            var userId = LessonData.UserId;

            _lessonRepoMock.Setup(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto));
            _lessonRepoMock.Setup(x => x.SelectByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var dto = sut.UpdateStudentAttendanceOnLesson(lessonId, userId, studentLessonDto);

            //Than
            Assert.AreEqual(studentLessonDto, dto);
            _lessonRepoMock.Verify(x => x.UpdateStudentAttendanceOnLesson(studentLessonDto), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectByLessonAndUserId(lessonId, userId), Times.Once);
        }

        [Test]
        public void GetAllFeedback_IntLessonId_ReturnedListStuentLessenDto()
        {
            //Given
            var lessonId = LessonData.LessonId;
            var listStudentLessonDto = LessonData.GetListStudentDto();

            _lessonRepoMock.Setup(x => x.SelectAllFeedbackByLessonId(lessonId)).Returns(listStudentLessonDto);
             
            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var listOfDto = sut.SelectAllFeedbackByLessonId(lessonId);

            //Than
            Assert.AreEqual(listStudentLessonDto, listOfDto);
            _lessonRepoMock.Verify(x => x.SelectAllFeedbackByLessonId(lessonId), Times.Once);
        }
        [Test]
        public void GetStudenLessonByLessonAndUserId_IntLessonIdAndUserId_ReturnStudentLessontDto()
        {
            //Given
            var studentLessonDto = LessonData.GetStudentLessonDto();
            var lessonId = LessonData.LessonId;
            var userId = LessonData.UserId;
                        
            _lessonRepoMock.Setup(x => x.SelectByLessonAndUserId(lessonId, userId)).Returns(studentLessonDto);

            var sut = new LessonService(_lessonRepoMock.Object, _commentRepoMock.Object, _userRepoMock.Object,
                _userValidationHelperMock.Object, _lessonValidationHelperMock.Object);

            //When
            var dto = sut.GetStudenLessonByLessonAndUserId(lessonId, userId);

            //Than
            Assert.AreEqual(studentLessonDto, dto);            
            _lessonRepoMock.Verify(x => x.SelectByLessonAndUserId(lessonId, userId), Times.Once);
        }



    }
}