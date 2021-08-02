﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.Services;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Repositories;
using Moq;
using NUnit.Framework;
using System;

namespace DevEdu.Business.Tests
{
    public class CommentServiceTests
    {
        private Mock<ICommentRepository> _commentRepoMock;
        private Mock<ILessonRepository> _lessonRepoMock;
        private Mock<IStudentAnswerOnTaskRepository> _studentAnswerRepoMock;
        private Mock<IGroupRepository> _groupRepoMock;
        private CommentValidationHelper _commentValidationHelper;
        private LessonValidationHelper _lessonValidationHelper;
        private StudentAnswerOnTaskValidationHelper _studentAnswerValidationHelper;
        private CommentService _sut;

        [SetUp]
        public void Setup()
        {
            _commentRepoMock = new Mock<ICommentRepository>();
            _lessonRepoMock = new Mock<ILessonRepository>();
            _groupRepoMock = new Mock<IGroupRepository>();
            _studentAnswerRepoMock = new Mock<IStudentAnswerOnTaskRepository>();
            _commentValidationHelper = new CommentValidationHelper(_commentRepoMock.Object);
            _lessonValidationHelper = new LessonValidationHelper(_lessonRepoMock.Object, _groupRepoMock.Object);
            _studentAnswerValidationHelper = new StudentAnswerOnTaskValidationHelper(_studentAnswerRepoMock.Object, _groupRepoMock.Object);
            _sut = new CommentService
            (
                _commentRepoMock.Object
                , _commentValidationHelper,
                _lessonValidationHelper,
                _studentAnswerValidationHelper
            );
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToLesson_CommentDtoAndExistingLessonInPassed_AddCommentAndReturned(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int lessonId = 1;
            const int expectedCommentId = 1;
            var lessonDto = CommentData.GetLessonDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;

            _commentRepoMock.Setup(x => x.AddComment(commentDto)).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetComment(expectedCommentId)).Returns(commentDto);
            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(lessonId)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(CommentData.GetGroupsDto());

            //When
            var actualComment = _sut.AddCommentToLesson(lessonId, commentDto, userToken);

            //Than
            Assert.AreEqual(commentDto, actualComment);
            _commentRepoMock.Verify(x => x.AddComment(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetComment(expectedCommentId), Times.Once);
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToStudentAnswer_CommentDtoAndExistingStudentAnswerIdPassed_CommentReturned(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int taskStudentId = 1;
            const int expectedCommentId = 1;
            const int expectedStudentAnswerOnTaskId = 1;
            var studentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;

            _commentRepoMock.Setup(x => x.AddComment(commentDto)).Returns(expectedCommentId);
            _commentRepoMock.Setup(x => x.GetComment(expectedCommentId)).Returns(commentDto);
            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswerOnTaskById(expectedStudentAnswerOnTaskId)).Returns(studentAnswerOnTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(CommentData.GetGroupsDto());

            //When
            var actualComment = _sut.AddCommentToStudentAnswer(taskStudentId, commentDto, userToken);

            //Than
            Assert.AreEqual(commentDto, actualComment);
            _commentRepoMock.Verify(x => x.AddComment(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetComment(expectedCommentId), Times.Once);
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswerOnTaskById(expectedStudentAnswerOnTaskId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void GetComment_ExistingCommentIdPassed_GetComment(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            const int commentId = 1;

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(commentDto.Lesson.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(CommentData.GetGroupsDto());

            //When
            var dto = _sut.GetComment(commentId, userToken);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(commentDto.Lesson.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void UpdateComment_CommentDtoAndExistingCommentIdPassed_ReturnUpdatedCommentDto(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            const int commentId = 1;

            _commentRepoMock.Setup(x => x.UpdateComment(commentDto));
            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(commentDto.Lesson.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(CommentData.GetGroupsDto());

            //When
            var dto = _sut.UpdateComment(commentId, commentDto, userToken);

            //Than
            Assert.AreEqual(commentDto, dto);
            _commentRepoMock.Verify(x => x.UpdateComment(commentDto), Times.Once);
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Exactly(2));
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(commentDto.Lesson.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteComment_ExistingCommentIdPassed_DeleteComment(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            const int commentId = 1;

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);
            _commentRepoMock.Setup(x => x.DeleteComment(commentId));
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(commentDto.Lesson.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(CommentData.GetGroupsDto());

            //When
            _sut.DeleteComment(commentId, userToken);

            //Than
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Once);
            _commentRepoMock.Verify(x => x.DeleteComment(commentId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(commentDto.Lesson.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToLesson_WhenLessonIdDoNotHaveMatchesInDataBase_EntityNotFoundException(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var lesson = CommentData.GetLessonDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(lesson), lesson.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddCommentToLesson(lesson.Id, commentDto, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToLesson_WhenUserDoNotHaveAccess_AuthorizationException(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int lessonId = 1;
            var lessonDto = CommentData.GetLessonDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            var expectedException = string.Format(ServiceMessages.UserOnLessonNotFoundMessage, userId, lessonId);

            _lessonRepoMock.Setup(x => x.SelectLessonById(lessonId)).Returns(lessonDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(lessonId)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto());

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.AddCommentToLesson(lessonId, commentDto, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _lessonRepoMock.Verify(x => x.SelectLessonById(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(lessonId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToStudentAnswer_WhenStudentAnswerIdDoNotHaveMatchesInDataBase_EntityNotFoundException(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            const int taskStudentId = 1;
            var studentAnswerOnTask = CommentData.GetStudentAnswerOnTaskDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(studentAnswerOnTask), studentAnswerOnTask.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.AddCommentToStudentAnswer(taskStudentId, commentDto, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void AddCommentToStudentAnswer_WhenUserDoNotHaveAccess_AuthorizationException(Enum role)
        {
            //Given
            var studentAnswerOnTaskDto = CommentData.GetStudentAnswerOnTaskDto();
            var commentDto = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            var expectedException = string.Format(ServiceMessages.UserHasNoAccessMessage, userId);

            _studentAnswerRepoMock.Setup(x => x.GetStudentAnswerOnTaskById(studentAnswerOnTaskDto.Id)).Returns(studentAnswerOnTaskDto);
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(studentAnswerOnTaskDto.User.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto());

            //When
            var ex = Assert.Throws<AuthorizationException>(
                () => _sut.AddCommentToStudentAnswer(studentAnswerOnTaskDto.Id, commentDto, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _studentAnswerRepoMock.Verify(x => x.GetStudentAnswerOnTaskById(studentAnswerOnTaskDto.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void GetComment_WhenCommentIdDoNotHaveMatchesInDataBase_EntityNotFoundException(Enum role)
        {
            //Given
            var comment = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(comment), comment.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.GetComment(comment.Id, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void GetComment_WhenUserDoNotHaveAccess_AuthorizationException(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            const int commentId = 1;
            var expectedException = string.Format(ServiceMessages.UserOnLessonNotFoundMessage, userId, commentId);

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(commentDto.Lesson.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto());

            //When
            var ex = Assert.Throws<AuthorizationException>(
                    () => _sut.GetComment(commentId, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(commentDto.Lesson.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void UpdateComment_WhenCommentIdDoNotHaveMatchesInDataBase_EntityNotFoundException(Enum role)
        {
            //Given
            var comment = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(comment), comment.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.UpdateComment(comment.Id, comment, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void UpdateComment_WhenUserDoNotHaveAccess_AuthorizationException(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            const int commentId = 1;
            var expectedException = string.Format(ServiceMessages.UserOnLessonNotFoundMessage, userId, commentId);

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(commentDto.Lesson.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto());

            //When
            var ex = Assert.Throws<AuthorizationException>(
                    () => _sut.UpdateComment(commentId, commentDto, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(commentDto.Lesson.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteComment_WhenCommentIdDoNotHaveMatchesInDataBase_EntityNotFoundException(Enum role)
        {
            //Given
            var comment = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var expectedException = string.Format(ServiceMessages.EntityNotFoundMessage, nameof(comment), comment.Id);

            //When
            var ex = Assert.Throws<EntityNotFoundException>(
                () => _sut.DeleteComment(comment.Id, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
        }

        [TestCase(Role.Teacher)]
        [TestCase(Role.Tutor)]
        [TestCase(Role.Student)]
        public void DeleteComment_WhenUserDoNotHaveAccess_AuthorizationException(Enum role)
        {
            //Given
            var commentDto = CommentData.GetCommentDto();
            var userToken = UserTokenData.GetUserIdentityWithRole(role);
            var userId = userToken.UserId;
            const int commentId = 1;
            var expectedException = string.Format(ServiceMessages.UserOnLessonNotFoundMessage, userId, commentId);

            _commentRepoMock.Setup(x => x.GetComment(commentId)).Returns(commentDto);
            _groupRepoMock.Setup(x => x.GetGroupsByLessonId(commentDto.Lesson.Id)).Returns(CommentData.GetGroupsDto());
            _groupRepoMock.Setup(x => x.GetGroupsByUserId(userId)).Returns(GroupData.GetGroupsDto());

            //When
            var ex = Assert.Throws<AuthorizationException>(
                    () => _sut.DeleteComment(commentId, userToken));

            //Than
            Assert.That(ex.Message, Is.EqualTo(expectedException));
            _commentRepoMock.Verify(x => x.GetComment(commentId), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByLessonId(commentDto.Lesson.Id), Times.Once);
            _groupRepoMock.Verify(x => x.GetGroupsByUserId(userId), Times.Once);
        }
    }
}