using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentValidationHelper _commentValidationHelper;
        private readonly ILessonValidationHelper _lessonValidationHelper;
        private readonly IStudentAnswerOnTaskValidationHelper _studentAnswerValidationHelper;

        public CommentService
        (
            ICommentRepository commentRepository,
            ICommentValidationHelper commentValidationHelper,
            ILessonValidationHelper lessonValidationHelper,
            IStudentAnswerOnTaskValidationHelper studentAnswerValidationHelper
        )
        {
            _commentRepository = commentRepository;
            _commentValidationHelper = commentValidationHelper;
            _lessonValidationHelper = lessonValidationHelper;
            _studentAnswerValidationHelper = studentAnswerValidationHelper;
        }

        public CommentDto AddCommentToLesson(int lessonId, CommentDto dto, UserIdentityInfo userInfo)
        {
            var userId = userInfo.UserId;
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            if (!userInfo.IsAdmin())
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);

            dto.User = new UserDto { Id = userId };
            dto.Lesson = new LessonDto { Id = lessonId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto, UserIdentityInfo userInfo)
        {
            var userId = userInfo.UserId;
            var studentAnswer = _studentAnswerValidationHelper.CheckStudentAnswerOnTaskExistence(taskStudentId);
            var studentId = studentAnswer.User.Id;
            if (!userInfo.IsAdmin())
                _studentAnswerValidationHelper.CheckUserInStudentAnswerAccess(studentId, userId);

            dto.StudentAnswer = new StudentAnswerOnTaskDto { Id = taskStudentId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto GetComment(int commentId, UserIdentityInfo userInfo)
        {
            var checkedDto = _commentValidationHelper.GetCommentByIdAndThrowIfNotFound(commentId);
            CheckUserAccessByRoleAndId(userInfo, checkedDto);
            return checkedDto;
        }

        public void DeleteComment(int commentId, UserIdentityInfo userInfo)
        {
            var checkedDto = _commentValidationHelper.GetCommentByIdAndThrowIfNotFound(commentId);
            CheckUserAccessByRoleAndId(userInfo, checkedDto);
            _commentRepository.DeleteComment(commentId);
        }

        public CommentDto UpdateComment(int commentId, CommentDto dto, UserIdentityInfo userInfo)
        {
            var checkedDto = _commentValidationHelper.GetCommentByIdAndThrowIfNotFound(commentId);
            CheckUserAccessByRoleAndId(userInfo, checkedDto);
            dto.Id = commentId;
            _commentRepository.UpdateComment(dto);
            return _commentRepository.GetComment(commentId);
        }

        private void CheckUserAccessByRoleAndId(UserIdentityInfo userInfo, CommentDto dto)
        {
            var userId = userInfo.UserId;

            if (userInfo.IsAdmin())
            {
                return;
            }

            CheckUserAccessToGroupData(dto, userId);

            if (userInfo.IsStudent())
            {
                _commentValidationHelper.UserComplianceCheck(dto, userId);
            }
        }

        private void CheckUserAccessToGroupData(CommentDto dto, int userId)
        {
            if (dto.Lesson != default)
            {
                var lessonId = dto.Lesson.Id;
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);
            }
            else
            {
                var studentId = dto.StudentAnswer.Id;
                _studentAnswerValidationHelper.CheckUserInStudentAnswerAccess(studentId, userId);
            }
        }
    }
}