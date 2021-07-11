using System.Data;
using Dapper;
using DevEdu.DAL.Models;


namespace DevEdu.DAL.Repositories
{
    public class LessonRepository : BaseRepository, ILessonRepository
    {
        private const string _lessonAddProcedure = "dbo.Lesson_Insert";
        private const string _lessonDeleteProcedure = "dbo.Lesson_Delete";
        private const string _lessonSelectAllProcedure = "dbo.Lesson_SelectAll";
        private const string _lessonSelectByIdProcedure = "dbo.Lesson_SelectById";
        private const string _lessonUpdateProcedure = "dbo.Lesson_Update";

        private const string _commentAddToLessonProcedure = "dbo.Lesson_Comment_Insert";
        private const string _commentDeleteFromLessonProcedure = "dbo.Lesson_Comment_Delete";
        private const string _topicAddToLessonProcedure = "dbo.Lesson_Topic_Insert";
        private const string _topicDeleteFromLessonProcedure = "dbo.Lesson_Topic_Delete";

        private const string _addStudentToLesson = "dbo.Student_Lesson_Insert";
        private const string _deleteStudentFromLesson = "dbo.Student_Lesson_Delete";
        private const string _lessonUpdateFeedbackProcedure = "dbo.Student_Lesson_UpdateFeedback";
        private const string _lessonUpdateAbsenceReasonProcedure = "dbo.Student_Lesson_UpdateAbsenceReason";
        private const string _lessonUpdateIsPresentProcedure = "dbo.Student_Lesson_UpdateIsPresent";

        public LessonRepository()
        {
        }

        public int AddLesson(LessonDto lessonDto)
        {
            return _connection.QueryFirst<int>(
               _lessonAddProcedure,
                new
                {
                    lessonDto.Date,
                    lessonDto.TeacherComment,
                    lessonDto.TeacherDto
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteLesson(int id)
        {
            _connection.Execute(
                _lessonDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public int AddCommentToLesson(int lessonId, int commentId)
        {
            return _connection.QueryFirst<int>(
                _commentAddToLessonProcedure,
                new
                {
                    lessonId,
                    commentId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteCommentFromLesson(int lessonId, int commentId)
        {
            _connection.Execute(
                _commentDeleteFromLessonProcedure,
                new
                {
                    lessonId,
                    commentId
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public int DeleteTopicFromLesson(int lessonId, int topicId)
        {
            return _connection.Execute(
                _topicDeleteFromLessonProcedure,
                new
                {
                    lessonId,
                    topicId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void AddTopicToLesson(int lessonId, int topicId)
        {
            _connection.Execute(
                _topicAddToLessonProcedure,
                new
                {
                    lessonId,
                    topicId
                },
                commandType: CommandType.StoredProcedure
            );
        }



        public void AddStudentToLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
            _addStudentToLesson,
             new
             {
                 LessonId = studentLessonDto.Lesson.Id,
                 UserId = studentLessonDto.User.Id
             },
             commandType: CommandType.StoredProcedure
         );
        }

        public void DeleteStudentFromLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
            _deleteStudentFromLesson,
             new
             {

                 LessonId = studentLessonDto.Lesson.Id,
                 UserId = studentLessonDto.User.Id
             },
             commandType: CommandType.StoredProcedure
         );
        }

        public void UpdateStudentFeedbackForLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
            _lessonUpdateFeedbackProcedure,
             new
             {
                 studentLessonDto.Feedback,
                 LessonId = studentLessonDto.Lesson.Id,
                 UserId = studentLessonDto.User.Id
             },
             commandType: CommandType.StoredProcedure
         );
        }

        public void UpdateStudentAbsenceReasonOnLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
            _lessonUpdateAbsenceReasonProcedure,
             new
             {
                 studentLessonDto.AbsenceReason,
                 LessonId = studentLessonDto.Lesson.Id,
                 UserId = studentLessonDto.User.Id
             },
             commandType: CommandType.StoredProcedure
         );
        }

        public void UpdateStudentAttendanceOnLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
            _lessonUpdateIsPresentProcedure,
             new
             {
                 studentLessonDto.IsPresent,
                 LessonId = studentLessonDto.Lesson.Id,
                 UserId = studentLessonDto.User.Id
             },
             commandType: CommandType.StoredProcedure
         );
        }
    }
}

