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
        private Mock<ICommentValidationHelper> _commentValidationHelper;

        [SetUp]
        public void Setup()
        {
            _commentRepoMock = new Mock<ICommentRepository>();
            _commentValidationHelper = new Mock<ICommentValidationHelper>();
        }

        [Test]
        public void AddCommentToLesson_CommentDto_CommentReturned()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var lessonId = 42;
            var expectedCommentId = 1;

            _commentRepoMock.Setup(x => x.AddComment(commentDto)).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetComment(expectedCommentId)).Returns(commentDto);

            var sut = new CommentService(_commentRepoMock.Object, _commentValidationHelper.Object);

            //When
            var actualComment = sut.AddCommentToLesson(lessonId, commentDto);

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
            var taskStudentId = 42;
            var expectedCommentId = 1;

            _commentRepoMock.Setup(x => x.AddComment(commentDto)).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetComment(expectedCommentId)).Returns(commentDto);

            var sut = new CommentService(_commentRepoMock.Object, _commentValidationHelper.Object);

            //When
            var actualComment = sut.AddCommentToLesson(taskStudentId, commentDto);

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
            var commentId = 42;

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);

            var sut = new CommentService(_commentRepoMock.Object, _commentValidationHelper.Object);

            //When
            var dto = sut.GetComment(commentId);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Once);
        }

        [Test]
        public void UpdateComment_CommentDto_ReturnUpdatedCommentDto()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var commentId = 42;

            _commentRepoMock.Setup(x => x.UpdateComment(commentDto));
            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);

            var sut = new CommentService(_commentRepoMock.Object, _commentValidationHelper.Object);

            //When
            var dto = sut.UpdateComment(commentId, commentDto);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.UpdateComment(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Once);
        }

        [Test]
        public void DeleteComment_IntCommentId_DeleteComment()
        {
            //Given
            var commentId = 42;

            _commentRepoMock.Setup(x => x.DeleteComment(commentId));

            var sut = new CommentService(_commentRepoMock.Object, _commentValidationHelper.Object);

            //When
            sut.DeleteComment(commentId);

            //Than
            _commentRepoMock.Verify(x => x.DeleteComment(commentId), Times.Once);
        }
    }
}