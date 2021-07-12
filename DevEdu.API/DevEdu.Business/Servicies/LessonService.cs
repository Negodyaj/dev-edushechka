using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class LessonService : ILessonService
    {
        private readonly ILessonRepository _lessonRepository;

        public LessonService(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }
              
        public int AddCommentToLesson(int lessonId, int commentId)
        {
            return _lessonRepository.AddCommentToLesson(lessonId, commentId);
        }

        public int AddLesson(LessonDto lessonDto)
        {
            return _lessonRepository.AddLesson(lessonDto);
        }

        public void DeleteCommentFromLesson(int lessonId, int commentId)
        {
            _lessonRepository.DeleteCommentFromLesson(lessonId, commentId);
        }

        public void DeleteLesson(int id)
        {
            _lessonRepository.DeleteLesson(id);
        }


        public void AddStudentToLesson(int lessonId, int userId)
        {
            var studentLessonDto =
               new StudentLessonDto
               {
                   User = new UserDto { Id = userId },
                   Lesson = new LessonDto { Id = lessonId }
               };
            _lessonRepository.AddStudentToLesson(studentLessonDto);
        }

        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
            var studentLessonDto =
                new StudentLessonDto
                {
                    User = new UserDto { Id = userId },
                    Lesson = new LessonDto { Id = lessonId }
                };
            _lessonRepository.DeleteStudentFromLesson(studentLessonDto);
        }
        public void UpdateStudentFeedbackForLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentFeedbackForLesson(studentLessonDto);
        }

        public void UpdateStudentAbsenceReasonOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAbsenceReasonOnLesson(studentLessonDto);
        }

        public void UpdateStudentAttendanceOnLesson(int lessonId, int userId, StudentLessonDto studentLessonDto)
        {
            studentLessonDto.Lesson = new LessonDto { Id = lessonId };
            studentLessonDto.User = new UserDto { Id = userId };
            _lessonRepository.UpdateStudentAttendanceOnLesson(studentLessonDto);
        }
    }
}
