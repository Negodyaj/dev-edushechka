using System.Data;
using Dapper;
using System.Collections.Generic;
using DevEdu.DAL.Models;
using System.Linq;

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

        private const string _studentLessonInsertProcedure = "dbo.Student_Lesson_Insert";
        private const string _studentLessonDeleteProcedure = "dbo.Student_Lesson_Delete";
        private const string _updateFeedbackProcedure = "dbo.Student_Lesson_UpdateFeedback";
        private const string _updateAbsenceReasonProcedure = "dbo.Student_Lesson_UpdateAbsenceReason";
        private const string _updateIsPresentProcedure = "dbo.Student_Lesson_UpdateIsPresent";
        private const string _selectAllFeedbackByLessonIdProcedure = "dbo.Student_Lesson_SelectAllFeedbackByLessonId";

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
                    lessonDto.Teacher.Id
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

        public void AddCommentToLesson(int lessonId, int commentId)
        {
            _connection.QueryFirst<int>(
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

        public List<LessonDto> SelectAllLessons()
        {
            return _connection
                .Query<LessonDto, UserDto, LessonDto>(
                    _lessonSelectAllProcedure,
                    (lesson, teacher) =>
                    {
                        lesson.Teacher = teacher;
                        return lesson;
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public LessonDto SelectLessonById(int id)
        {
            return _connection
                .Query<LessonDto, UserDto, LessonDto>(
                    _lessonSelectByIdProcedure,
                    (lesson, teacher) =>
                    {
                        lesson.Teacher = teacher;
                        return lesson;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public void UpdateLesson(LessonDto lessonDto)
        {
            _connection.QuerySingleOrDefault<int>(
               _lessonUpdateProcedure,
               new
               {
                   lessonDto.Id,
                   lessonDto.TeacherComment,
                   lessonDto.Date
               },
               commandType: CommandType.StoredProcedure
           );
        }

        public void AddStudentToLesson(StudentLessonDto dto)
        {
            _connection.Execute(
                _studentLessonInsertProcedure,
                 new
                 {
                     LessonId = dto.Lesson.Id,
                     UserId = dto.User.Id
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void DeleteStudentFromLesson(StudentLessonDto dto)
        {
            _connection.Execute(
                _studentLessonDeleteProcedure,
                 new
                 {
                     LessonId = dto.Lesson.Id,
                     UserId = dto.User.Id
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void UpdateStudentFeedbackForLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
                _updateFeedbackProcedure,
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
                _updateAbsenceReasonProcedure,
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
                _updateIsPresentProcedure,
                 new
                 {
                     studentLessonDto.IsPresent,
                     LessonId = studentLessonDto.Lesson.Id,
                     UserId = studentLessonDto.User.Id
                 },
                 commandType: CommandType.StoredProcedure
             );
        }


        public List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId)
        {
            StudentLessonDto result;
            var list = _connection.Query<StudentLessonDto, UserDto, StudentLessonDto>(
                _selectAllFeedbackByLessonIdProcedure,
                (studentLesson, user) =>
                {
                    result = studentLesson;
                    result.User = user;
                    result.Feedback = studentLesson.Feedback;

                    return result;
                },
                  new { lessonId },
                  splitOn: "Id",
                  commandType: CommandType.StoredProcedure
                  )
                  .ToList();
            return list;
        }
    }
}


