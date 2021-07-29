using System.Security.Cryptography.X509Certificates;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class CommentServiceTests
    {
        private Mock<ICommentRepository> _commentRepoMock;
        private Mock<ILessonRepository> _lessonRepoMock;
        private Mock<IUserRepository> _userRepoMock;
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerRepoMock;
        private CommentValidationHelper _commentValidationHelper;
        private LessonValidationHelper _lessonValidationHelper;
        private StudentAnswerOnTaskValidationHelper _studentAnswerValidationHelper;
        private CommentService _sut;

        [SetUp]
        public void Setup()
        {
            _commentRepoMock = new Mock<ICommentRepository>();
            _lessonRepoMock = new Mock<ILessonRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _commentValidationHelper = new CommentValidationHelper(_commentRepoMock.Object);
            _lessonValidationHelper = new LessonValidationHelper(_lessonRepoMock.Object);
            _studentAnswerValidationHelper = new StudentAnswerOnTaskValidationHelper(_studentAnswerRepoMock.Object);
            _sut = new CommentService(_commentRepoMock.Object, _commentValidationHelper, _lessonValidationHelper, _studentAnswerValidationHelper);
        }

        [Test]
        public void AddCommentToLesson_CommentDto_CommentReturned()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var lessonId = 1;
            var expectedCommentId = 1;
            var lessonDto = CommentData.GetLessonDto();
            var userId = 1;
            var studentLessonDto = CommentData.GetStudentLessonDto();

            _commentRepoMock.Setup(x => x.AddComment(commentDto)).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetComment(expectedCommentId)).Returns(commentDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _lessonRepoMock.Setup(x => x.SelectByLessonAndUserId(lessonId,userId)).Returns(studentLessonDto);
           


            //When
            var actualComment = _sut.AddCommentToLesson(lessonId, commentDto);

            //Than
            Assert.AreEqual(commentDto, actualComment);
            _commentRepoMock.Verify(x => x.AddComment(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetComment(expectedCommentId), Times.Once);
        }

        [Test]
        public void AddCommentToStudentAnswer_CommentDto_CommentReturned()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var taskStudentId = 1;
            var expectedCommentId = 1;
            var expectedStudentAnswerOnTaskId = 1;
            var getStudentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();

            _commentRepoMock.Setup(x => x.AddComment(commentDto)).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetComment(expectedCommentId)).Returns(commentDto);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswerOnTaskById(expectedStudentAnswerOnTaskId)).Returns(getStudentAnswerOnTaskDto);

            //When
            var actualComment = _sut.AddCommentToStudentAnswer(taskStudentId, commentDto);

            //Than
            Assert.AreEqual(commentDto, actualComment);
            _commentRepoMock.Verify(x => x.AddComment(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetComment(expectedCommentId), Times.Once);
        }

        [Test]
        public void GetComment_IntCommentId_GetComment()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var commentId = 1;

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);

            //When
            var dto = _sut.GetComment(commentId);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Exactly(2));
        }

        [Test]
        public void UpdateComment_CommentDto_ReturnUpdatedCommentDto()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var commentId = 1;

            _commentRepoMock.Setup(x => x.UpdateComment(commentDto));
            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);

            //When
            var dto = _sut.UpdateComment(commentId, commentDto);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.UpdateComment(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Exactly(2));
        }

        [Test]
        public void DeleteComment_IntCommentId_DeleteComment()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var commentId = 1;

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);
            _commentRepoMock.Setup(x => x.DeleteComment(commentId));

            //When
            _sut.DeleteComment(commentId);

            //Than
            _commentRepoMock.Verify(x => x.DeleteComment(commentId), Times.Once);
        }
    }
}