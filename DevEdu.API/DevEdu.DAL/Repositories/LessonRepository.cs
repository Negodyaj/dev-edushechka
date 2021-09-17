using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class LessonRepository : BaseRepository, ILessonRepository
    {
        private const string _lessonInsertProcedure = "dbo.Lesson_Insert";
        private const string _lessonDeleteProcedure = "dbo.Lesson_Delete";
        private const string _lessonSelectAllByGroupIdProcedure = "dbo.Lesson_SelectAllByGroupId";
        private const string _lessonSelectAllByTeacherIdProcedure = "dbo.Lesson_SelectAllByTeacherId";
        private const string _lessonSelectByIdProcedure = "dbo.Lesson_SelectById";
        private const string _lessonUpdateProcedure = "dbo.Lesson_Update";

        private const string _lessonTopicInsertProcedure = "dbo.Lesson_Topic_Insert";
        private const string _lessonTopicDeleteProcedure = "dbo.Lesson_Topic_Delete";

        private const string _studentLessonInsertProcedure = "dbo.Student_Lesson_Insert";
        private const string _studentLessonDeleteProcedure = "dbo.Student_Lesson_Delete";
        private const string _studentLessonUpdateFeedbackProcedure = "dbo.Student_Lesson_UpdateFeedback";
        private const string _studentLessonUpdateAbsenceReasonProcedure = "dbo.Student_Lesson_UpdateAbsenceReason";
        private const string _studentLessonUpdateIsPresentProcedure = "dbo.Student_Lesson_UpdateIsPresent";
        private const string _studentLessonSelectAllFeedbackByLessonIdProcedure = "dbo.Student_Lesson_SelectAllFeedbackByLessonId";
        private const string _studentLessonSelectByLessonAndUserIdProcedure = "dbo.Student_Lesson_SelectByLessonAndUserId";

        public LessonRepository(IOptions<DatabaseSettings> options) : base(options) { }

        public int AddLesson(LessonDto lessonDto)
        {
            return _connection.QueryFirst<int>(
                _lessonInsertProcedure,
                new
                {
                    lessonDto.Date,
                    lessonDto.TeacherComment,
                    TeacherId = lessonDto.Teacher.Id,
                    lessonDto.LinkToRecord
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

        public async Task<int> DeleteTopicFromLessonAsync(int lessonId, int topicId)
        {
            return await _connection.ExecuteAsync(
                _lessonTopicDeleteProcedure,
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
                _lessonTopicInsertProcedure,
                new
                {
                    lessonId,
                    topicId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<LessonDto>> SelectAllLessonsByGroupIdAsync(int groupId)
        {
            var lessonDictionary = new Dictionary<int, LessonDto>();

            var list = (await _connection
                .QueryAsync<LessonDto, UserDto, TopicDto, LessonDto>(
                    _lessonSelectAllByGroupIdProcedure,
                    (lesson, teacher, topic) =>
                    {
                        if (!lessonDictionary.TryGetValue(lesson.Id, out var lessonEntry))
                        {
                            lessonEntry = lesson;
                            lessonEntry.Teacher = teacher;
                            lessonEntry.Topics = new List<TopicDto>();
                            lessonDictionary.Add(lessonEntry.Id, lessonEntry);
                        }

                        lessonEntry.Topics.Add(topic);
                        return lessonEntry;
                    },
                    new { groupId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .Distinct()
                .ToList();

            return list;
        }

        public List<LessonDto> SelectAllLessonsByTeacherId(int teacherId)
        {
            var lessonDictionary = new Dictionary<int, LessonDto>();

            var list = _connection
               .Query<LessonDto, UserDto, TopicDto, CourseDto, LessonDto>(
                   _lessonSelectAllByTeacherIdProcedure,
                   (lesson, teacher, topic, course) =>
                   {
                       if (!lessonDictionary.TryGetValue(lesson.Id, out var lessonEntry))
                       {
                           lessonEntry = lesson;
                           lessonEntry.Teacher = teacher;
                           lessonEntry.Topics = new List<TopicDto>();
                           lessonEntry.Course = course;
                           lessonDictionary.Add(lessonEntry.Id, lessonEntry);
                       }

                       lessonEntry.Topics.Add(topic);
                       return lessonEntry;
                   },
                   new { teacherId },
                   splitOn: "Id",
                   commandType: CommandType.StoredProcedure
               )
               .Distinct()
               .ToList();

            return list;
        }

        public LessonDto SelectLessonById(int id)
        {
            LessonDto result = default;
            _connection
                .Query<LessonDto, UserDto, TopicDto, LessonDto>(
                    _lessonSelectByIdProcedure,
                    (lesson, teacher, topic) =>
                    {
                        if (result == null)
                        {
                            result = lesson;
                            result.Teacher = teacher;
                            result.Topics = new List<TopicDto>();
                        }
                        if (topic != null)
                            result.Topics.Add(topic);

                        return lesson;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                );
            return result;
        }

        public async Task<List<StudentLessonDto>> SelectStudentsLessonByLessonIdAsync(int lessonId)
        {
            return (await _connection
                .QueryAsync<StudentLessonDto, UserDto, StudentLessonDto>(
                    "[dbo].[Student_Lesson_SelectByLessonId]",
                    (studentLesson, user) =>
                    {
                        studentLesson.Student = user;
                        return studentLesson;
                    },
                    new { lessonId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task UpdateLessonAsync(LessonDto lessonDto)
        {
            await _connection.QuerySingleOrDefaultAsync<int>(
               _lessonUpdateProcedure,
               new
               {
                   lessonDto.Id,
                   lessonDto.TeacherComment,
                   lessonDto.LinkToRecord,
                   lessonDto.Date
               },
               commandType: CommandType.StoredProcedure
           );
        }

        public void AddStudentToLesson(int lessonId, int studentId)
        {
            _connection.Execute(
                _studentLessonInsertProcedure,
                 new
                 {
                     lessonId,
                     studentId
                 },
                 commandType: CommandType.StoredProcedure
             );

        }

        public void DeleteStudentFromLesson(int lessonId, int studentId)
        {
            _connection.Execute(
                _studentLessonDeleteProcedure,
                 new
                 {
                     lessonId,
                     studentId
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void UpdateStudentFeedbackForLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
                _studentLessonUpdateFeedbackProcedure,
                 new
                 {
                     studentLessonDto.Feedback,
                     LessonId = studentLessonDto.Lesson.Id,
                     StudentId = studentLessonDto.Student.Id
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void UpdateStudentAbsenceReasonOnLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
                _studentLessonUpdateAbsenceReasonProcedure,
                 new
                 {
                     studentLessonDto.AbsenceReason,
                     LessonId = studentLessonDto.Lesson.Id,
                     StudentId = studentLessonDto.Student.Id
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void UpdateStudentAttendanceOnLesson(StudentLessonDto studentLessonDto)
        {
            _connection.Execute(
                _studentLessonUpdateIsPresentProcedure,
                 new
                 {
                     studentLessonDto.IsPresent,
                     LessonId = studentLessonDto.Lesson.Id,
                     StudentId = studentLessonDto.Student.Id
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public List<StudentLessonDto> SelectAllFeedbackByLessonId(int lessonId)
        {
            StudentLessonDto result;
            var list = _connection.Query<StudentLessonDto, UserDto, StudentLessonDto>(
                _studentLessonSelectAllFeedbackByLessonIdProcedure,
                (studentLesson, user) =>
                {
                    result = studentLesson;
                    result.Student = user;
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


        public StudentLessonDto SelectAttendanceByLessonAndUserId(int lessonId, int studentId)
        {
            return _connection.Query<StudentLessonDto, LessonDto, UserDto, StudentLessonDto>(
                _studentLessonSelectByLessonAndUserIdProcedure,
                (studentLesson, lesson, user) =>
                {
                    var result = studentLesson;
                    result.Lesson = lesson;
                    result.Student = user;
                    return result;
                },
                new
                {
                    LessonId = lessonId,
                    StudentId = studentId
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            ).First();

        }
    }
}