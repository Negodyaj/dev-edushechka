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

        public CommentDto AddCommentToLesson(int lessonId, CommentDto dto)
        {
            _lessonValidationHelper.CheckLessonExistence(lessonId);
            var userId = dto.User.Id;
            _lessonValidationHelper.CheckUserInLessonExistence(lessonId, userId);
            dto.Lesson = new LessonDto { Id = lessonId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto)
        {
            _studentAnswerValidationHelper.CheckStudentAnswerOnTaskExistence(taskStudentId);
            dto.StudentAnswer = new StudentAnswerOnTaskDto { Id = taskStudentId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto GetComment(int id)
        {
            _commentValidationHelper.CheckCommentExistence(id);
            return _commentRepository.GetComment(id);
        }

        public void DeleteComment(int id)
        {
            _commentValidationHelper.CheckCommentExistence(id);
            _commentRepository.DeleteComment(id);
        }

        public CommentDto UpdateComment(int id, CommentDto dto)
        {
            _commentValidationHelper.CheckCommentExistence(id);
            dto.Id = id;
            _commentRepository.UpdateComment(dto);
            return _commentRepository.GetComment(id);
        }
    }
}