using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

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

        public CommentDto AddCommentToLesson(int lessonId, CommentDto dto, int userId, List<Role> roles)
        {
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            if (!CheckerRole.Admin(roles))
                _lessonValidationHelper.CheckUserInLessonAccess(lessonId, userId);

            dto.User = new UserDto { Id = userId };
            dto.Lesson = new LessonDto { Id = lessonId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto, int userId, List<Role> roles)
        {
            var studentAnswer = _studentAnswerValidationHelper.CheckStudentAnswerOnTaskExistence(taskStudentId);
            var studentId = studentAnswer.User.Id;
            if (!CheckerRole.Admin(roles))
                _studentAnswerValidationHelper.CheckUserInStudentAnswerAccess(studentId, userId);

            dto.StudentAnswer = new StudentAnswerOnTaskDto { Id = taskStudentId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto GetComment(int commentId, int userId, List<Role> roles)
        {
            return CheckerRole.Admin(roles) ? _commentRepository.GetComment(commentId) : CheckAccessAndExistence(commentId, userId);
        }

        public void DeleteComment(int commentId, int userId, List<Role> roles)
        {
            CheckAccess(commentId, userId, roles);
            _commentRepository.DeleteComment(commentId);
        }

        public CommentDto UpdateComment(int commentId, CommentDto dto, int userId, List<Role> roles)
        {
            CheckAccess(commentId, userId, roles);
            dto.Id = commentId;
            _commentRepository.UpdateComment(dto);
            return _commentRepository.GetComment(commentId);
        }

        private CommentDto CheckAccessAndExistence(int commentId, int userId)
        {
            var dto = _commentValidationHelper.CheckCommentExistence(commentId);
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
            return dto;
        }

        private void CheckAccess(int commentId, int userId, List<Role> roles)
        {
            if (CheckerRole.Admin(roles)) return;

            var checkedDto = CheckAccessAndExistence(commentId, userId);
            if (CheckerRole.Student(roles))
            {
                _commentValidationHelper.CheckUser(checkedDto, userId);
            }
        }
    }
}