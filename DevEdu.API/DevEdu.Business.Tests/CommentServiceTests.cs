using DevEdu.Business.Services;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;

namespace DevEdu.Business.Tests
{
    public class CommentServiceTests
    {
        private Mock<ICommentRepository> _commentRepoMock;

        [SetUp]
        public void Setup()
        {
            _commentRepoMock = new Mock<ICommentRepository>();
        }

        [Test]
        public void AddComment_CommentDto_CommentCreated()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();

            _commentRepoMock.Setup(x => x.AddComment(commentDto)).Returns(CommentData.ExpectedCommentId);

            var sut = new CommentService(_commentRepoMock.Object);

            //When
            var actualCommentId = sut.AddComment(commentDto);

            //Than
            Assert.AreEqual(CommentData.ExpectedCommentId, actualCommentId);
            _commentRepoMock.Verify(x => x.AddComment(commentDto), Times.Once);
        }

        [Test]
        public void GetComment_IntCommentId_GetComment()
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int commentId = CommentData.CommentId;

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);

            var sut = new CommentService(_commentRepoMock.Object);

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
            const int commentId = CommentData.CommentId;

            _commentRepoMock.Setup(x => x.UpdateComment(commentDto)).Returns(commentDto);

            var sut = new CommentService(_commentRepoMock.Object);

            //When
            var dto = sut.UpdateComment(commentId, commentDto);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.UpdateComment(commentDto), Times.Once);
        }

        [Test]
        public void DeleteComment_IntCommentId_DeleteComment()
        {
            //Given
            const int commentId = CommentData.CommentId;

            _commentRepoMock.Setup(x => x.DeleteComment(commentId));

            var sut = new CommentService(_commentRepoMock.Object);

            //When
            sut.DeleteComment(commentId);

            //Than
            _commentRepoMock.Verify(x => x.DeleteComment(commentId), Times.Once);
        }

        [Test]
        public void GetCommentByUserId_IntUserId_ReturnedListOfUserComments()
        {
            //Given
            var commentsList = CommentData.GetListCommentsDto();
            const int userId = CommentData.UserId;

            _commentRepoMock.Setup(x => x.GetCommentsByUser(userId)).Returns(commentsList);

            var sut = new CommentService(_commentRepoMock.Object);

            //When
            var listOfDto = sut.GetCommentsByUserId(userId);

            //Than
            Assert.AreEqual(commentsList, listOfDto);
            _commentRepoMock.Verify(x => x.GetCommentsByUser(userId), Times.Once);
        }
    }
}