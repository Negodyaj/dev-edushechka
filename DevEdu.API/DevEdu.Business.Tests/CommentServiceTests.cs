using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace DevEdu.Business.Tests
{
    public class CommentServiceTests
    {
        private Mock<ICommentRepository> _commentRepoMock;
        private Mock<ILessonRepository> _lessonRepoMock;
        private Mock<IStudentHomeworkRepository> _studentAnswerRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private CommentValidationHelper _commentValidationHelper;
        private LessonValidationHelper _lessonValidationHelper;
        private StudentHomeworkValidationHelper _studentAnswerValidationHelper;
        private CommentService _sut;

        [SetUp]
        public void Setup()
        {
            _commentRepoMock = new Mock<ICommentRepository>();
            _lessonRepoMock = new Mock<ILessonRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _studentAnswerRepoMock = new Mock<IStudentHomeworkRepository>();
            _commentValidationHelper = new CommentValidationHelper(_commentRepoMock.Object);
            _lessonValidationHelper = new LessonValidationHelper(_lessonRepoMock.Object, _groupRepoMock.Object);
            _studentAnswerValidationHelper = new StudentHomeworkValidationHelper(_studentAnswerRepoMock.Object, _groupRepoMock.Object);
            _sut = new CommentService
            (
                _commentRepoMock.Object,
                _commentValidationHelper,
                _lessonValidationHelper,
                _studentAnswerValidationHelper
            );
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public async Task AddCommentToLesson_CommentDtoAndExistingLessonInPassed_AddCommentAndReturnedAsync(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int lessonId = 1;
            const int expectedCommentId = 1;
            var lessonDto = CommentData.GetLessonDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;

            _commentRepoMock.Setup(x => x.AddCommentAsync(commentDto).Result).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetCommentAsync(expectedCommentId).Result).Returns(commentDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonIdAsync(lessonId)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(CommentData.GetGroupsDto());

            //When
            var actualComment = await _sut.AddCommentToLessonAsync(lessonId, commentDto, userInfo);

            //Than
            Assert.AreEqual(commentDto, actualComment);
            _commentRepoMock.Verify(x => x.AddCommentAsync(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetCommentAsync(expectedCommentId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonIdAsync(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public async Task AddCommentToStudentAnswer_CommentDtoAndExistingStudentAnswerIdPassed_CommentReturned(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int taskStudentId = 1;
            const int expectedCommentId = 1;
            const int expectedStudentAnswerOnTaskId = 1;
            var studentAnswerOnTaskDto = CommentData.GetStudentHomeworkDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;

            _commentRepoMock.Setup(x => x.AddCommentAsync(commentDto).Result).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetCommentAsync(expectedCommentId).Result).Returns(commentDto);
            _studentAnswerRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(expectedStudentAnswerOnTaskId)).ReturnsAsync(studentAnswerOnTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentAnswerOnTaskDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(CommentData.GetGroupsDto());

            //When
            var actualComment = await _sut.AddCommentToStudentHomeworkAsync(taskStudentId, commentDto, userInfo);

            //Than
            Assert.AreEqual(commentDto, actualComment);
            _commentRepoMock.Verify(x => x.AddCommentAsync(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetCommentAsync(expectedCommentId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(expectedStudentAnswerOnTaskId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(studentAnswerOnTaskDto.User.Id), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public async Task GetComment_ExistingCommentIdPassed_CommentReturnedAsync(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            const int commentId = 1;

            _commentRepoMock.Setup(x => x.GetCommentAsync(commentId).Result).Returns(commentDto);

            //When
            var dto = await _sut.GetCommentAsync(commentId, userInfo);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.GetCommentAsync(commentId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public async Task UpdateComment_CommentDtoAndExistingCommentIdPassed_ReturnUpdatedCommentDtoAsync(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            const int commentId = 1;

            _commentRepoMock.Setup(x => x.UpdateCommentAsync(commentDto));
            _commentRepoMock.Setup(x => x.GetCommentAsync(commentId).Result).Returns(commentDto);

            //When
            var dto = await _sut.UpdateCommentAsync(commentId, commentDto, userInfo);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.UpdateCommentAsync(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetCommentAsync(commentId), Times.Exactly(2));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public async Task DeleteComment_ExistingCommentIdPassed_CommentRemovedAsync(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            const int commentId = 1;

            _commentRepoMock.Setup(x => x.GetCommentAsync(commentId).Result).Returns(commentDto);
            _commentRepoMock.Setup(x => x.DeleteCommentAsync(commentId));

            //When
            await _sut.DeleteCommentAsync(commentId, userInfo);

            //Than
            _commentRepoMock.Verify(x => x.GetCommentAsync(commentId), Times.Once);
            _commentRepoMock.Verify(x => x.DeleteCommentAsync(commentId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToLesson_WhenLessonIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var lesson = CommentData.GetLessonDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(lesson), lesson.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddCommentToLessonAsync(lesson.Id, commentDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToLesson_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int lessonId = 1;
            var lessonDto = CommentData.GetLessonDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            var expectedException = string.Format(ServiceMessages.UserDoesntBelongToLesson, userId, lessonId);

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonIdAsync(lessonId)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(GroupData.GetGroupDtos());

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.AddCommentToLessonAsync(lessonId, commentDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonIdAsync(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToStudentAnswer_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int taskStudentId = 1;
            var studentHomework = CommentData.GetStudentHomeworkDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentHomework), studentHomework.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.AddCommentToStudentHomeworkAsync(taskStudentId, commentDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToStudentAnswer_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role)
        {
            //Given
            var studentAnswerOnTaskDto = CommentData.GetStudentHomeworkDto();
            var commentDto = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var userId = userInfo.UserId;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerRepoMock.Setup(x => x.GetStudentHomeworkByIdAsync(studentAnswerOnTaskDto.Id)).ReturnsAsync(studentAnswerOnTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(studentAnswerOnTaskDto.User.Id)).ReturnsAsync(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserIdAsync(userId)).ReturnsAsync(GroupData.GetGroupDtos());

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                () => _sut.AddCommentToStudentHomeworkAsync(studentAnswerOnTaskDto.Id, commentDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _studentAnswerRepoMock.Verify(x => x.GetStudentHomeworkByIdAsync(studentAnswerOnTaskDto.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserIdAsync(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void GetComment_WhenCommentIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var comment = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(comment), comment.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.GetCommentAsync(comment.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher, 2)]
        [TestCase(Role.Tutor, 2)]
        [TestCase(Role.Student, 2)]
        public void GetComment_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            const int commentId = 1;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _commentRepoMock.Setup(x => x.GetCommentAsync(commentId).Result).Returns(commentDto);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                    () => _sut.GetCommentAsync(commentId, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _commentRepoMock.Verify(x => x.GetCommentAsync(commentId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void UpdateComment_WhenCommentIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var comment = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(comment), comment.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.UpdateCommentAsync(comment.Id, comment, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher, 2)]
        [TestCase(Role.Tutor, 2)]
        [TestCase(Role.Student, 2)]
        public void UpdateComment_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            const int commentId = 1;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _commentRepoMock.Setup(x => x.GetCommentAsync(commentId).Result).Returns(commentDto);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                    () => _sut.UpdateCommentAsync(commentId, commentDto, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _commentRepoMock.Verify(x => x.GetCommentAsync(commentId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteComment_WhenCommentIdDoNotHaveMatchesInDataBase_EntityNotFoundAndExceptionThrown(Enum role)
        {
            //Given
            var comment = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(comment), comment.Id);

            //When
            var ex = Assert.ThrowsAsync<EntityNotFoundException>(
                () => _sut.DeleteCommentAsync(comment.Id, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher, 2)]
        [TestCase(Role.Tutor, 2)]
        [TestCase(Role.Student, 2)]
        public void DeleteComment_WhenUserDoNotHaveAccess_AuthorizationExceptionThrown(Enum role, int userId)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userInfo = UserIdentityInfoData.GetUserIdentityWithRole(role, userId);
            const int commentId = 1;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _commentRepoMock.Setup(x => x.GetCommentAsync(commentId).Result).Returns(commentDto);

            //When
            var ex = Assert.ThrowsAsync<AuthorizationException>(
                    () => _sut.DeleteCommentAsync(commentId, userInfo));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _commentRepoMock.Verify(x => x.GetCommentAsync(commentId), Times.Once);
        }
    }
}