using System.Data;
using Dapper;
using DevEdu.DAL.Models;


namespace DevEdu.DAL.Repositories
{
    public class LessonRepository : BaseRepository, ILessonRepository
    {
        private const string _lessonInsertProcedure = "dbo.Student_Lesson_Insert";
        private const string _lessonDeleteProcedure = "dbo.Student_Lesson_Delete";
        private const string _lessonUpdateFeedbackProcedure = "dbo.Student_Lesson_UpdateFeedback";
        private const string _lessonUpdateAbsenceReasonProcedure = "dbo.Student_Lesson_UpdateAbsenceReason";
        private const string _lessonUpdateIsPresentProcedure = "dbo.Student_Lesson_UpdateIsPresent";

        public LessonRepository()
        {
        }

        public void AddStudentToLesson(int lessonId, int userId)
        {
            _connection.Execute(
            _lessonInsertProcedure,
             new
             {
                 lessonId,
                 userId
             },
             commandType: CommandType.StoredProcedure
         );
        }

        public void DeleteStudentFromLesson(int lessonId, int userId)
        {
            _connection.Execute(
            _lessonDeleteProcedure,
             new
             {
                 lessonId,
                 userId
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
                 studentLessonDto.LessonId,
                 studentLessonDto.UserId
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
                 studentLessonDto.LessonId,
                 studentLessonDto.UserId
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
                 studentLessonDto.LessonId,
                 studentLessonDto.UserId
             },
             commandType: CommandType.StoredProcedure
         );
        }
    }
}

