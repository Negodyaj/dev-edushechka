using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentValidationHelper _commentValidationHelper;
        private readonly ILessonValidationHelper _lessonValidationHelper;
        private readonly IStudentHomeworkValidationHelper _studentAnswerValidationHelper;

        public CommentService
        (
            ICommentRepository commentRepository,
            ICommentValidationHelper commentValidationHelper,
            ILessonValidationHelper lessonValidationHelper,
            IStudentHomeworkValidationHelper studentAnswerValidationHelper
        )
        {
            _commentRepository = commentRepository;
            _commentValidationHelper = commentValidationHelper;
            _lessonValidationHelper = lessonValidationHelper;
            _studentAnswerValidationHelper = studentAnswerValidationHelper;
        }

        public CommentDto AddCommentToLesson(int lessonId, CommentDto dto, UserIdentityInfo userInfo)
        {
            _lessonValidationHelper.GetLessonByIdAndThrowIfNotFound(lessonId);
            if (!userInfo.IsAdmin())
                _lessonValidationHelper.CheckUserBelongsToLesson(lessonId, userInfo.UserId);

            dto.User = new UserDto { Id = userInfo.UserId };
            dto.Lesson = new LessonDto { Id = lessonId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto, UserIdentityInfo userInfo)
        {
            var studentAnswer = _studentAnswerValidationHelper.GetStudentAnswerByIdAndThrowIfNotFound(taskStudentId);
            var studentId = studentAnswer.User.Id;
            if (!userInfo.IsAdmin())
                _studentAnswerValidationHelper.CheckUserInStudentHomeworkAccess(studentId, userInfo.UserId);

            dto.StudentAnswer = new StudentHomeworkDto { Id = taskStudentId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto GetComment(int commentId, UserIdentityInfo userInfo)
        {
            var checkedDto = _commentValidationHelper.GetCommentByIdAndThrowIfNotFound(commentId);
            CheckUserAccessToCommentByUserId(userInfo, checkedDto);
            return checkedDto;
        }

        public void DeleteComment(int commentId, UserIdentityInfo userInfo)
        {
            var checkedDto = _commentValidationHelper.GetCommentByIdAndThrowIfNotFound(commentId);
            CheckUserAccessToCommentByUserId(userInfo, checkedDto);
            _commentRepository.DeleteComment(commentId);
        }

        public CommentDto UpdateComment(int commentId, CommentDto dto, UserIdentityInfo userInfo)
        {
            var checkedDto = _commentValidationHelper.GetCommentByIdAndThrowIfNotFound(commentId);
            CheckUserAccessToCommentByUserId(userInfo, checkedDto);
            dto.Id = commentId;
            _commentRepository.UpdateComment(dto);
            return _commentRepository.GetComment(commentId);
        }

        private void CheckUserAccessToCommentByUserId(UserIdentityInfo userInfo, CommentDto dto)
        {
            var userId = userInfo.UserId;

            if (userInfo.IsAdmin())
            {
                return;
            }
            _commentValidationHelper.UserComplianceCheck(dto, userId);
        }
    }
}