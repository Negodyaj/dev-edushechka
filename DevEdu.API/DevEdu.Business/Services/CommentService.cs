using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ICommentValidationHelper _commentValidationHelper;

        public CommentService
        (
            ICommentRepository commentRepository,
            ICommentValidationHelper commentValidationHelper
        )
        {
            _commentRepository = commentRepository;
            _commentValidationHelper = commentValidationHelper;
        }

        public CommentDto AddCommentToLesson(int lessonId, CommentDto dto)
        {
            dto.Lesson = new LessonDto { Id = lessonId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto AddCommentToStudentAnswer(int taskStudentId, CommentDto dto)
        {
            dto.StudentAnswer = new StudentAnswerOnTaskDto { Id = taskStudentId };
            var id = _commentRepository.AddComment(dto);
            return _commentRepository.GetComment(id);
        }

        public CommentDto GetComment(int id)
        {
            //_commentValidationHelper.CheckCommentExistence(id);
            return _commentRepository.GetComment(id);
        }

        public void DeleteComment(int id) => _commentRepository.DeleteComment(id);

        public CommentDto UpdateComment(int id, CommentDto dto)
        {
            dto.Id = id;
            _commentRepository.UpdateComment(dto);
            return _commentRepository.GetComment(id);
        }
    }
}