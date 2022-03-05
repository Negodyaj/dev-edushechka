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

        private const string _studentLessonSelectByLessonIdProcedure = "dbo.Student_Lesson_SelectByLessonId";
        private const string _studentLessonInsertProcedure = "dbo.Student_Lesson_Insert";
        private const string _studentLessonDeleteProcedure = "dbo.Student_Lesson_Delete";
        private const string _studentLessonUpdateFeedbackProcedure = "dbo.Student_Lesson_UpdateFeedback";
        private const string _studentLessonUpdateAbsenceReasonProcedure = "dbo.Student_Lesson_UpdateAbsenceReason";
        private const string _studentLessonUpdateIsPresentProcedure = "dbo.Student_Lesson_UpdateIsPresent";
        private const string _studentLessonSelectAllFeedbackByLessonIdProcedure = "dbo.Student_Lesson_SelectAllFeedbackByLessonId";
        private const string _studentLessonSelectByLessonAndUserIdProcedure = "dbo.Student_Lesson_SelectByLessonAndUserId";

        public LessonRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddLessonAsync(LessonDto lessonDto)
        {
            return await _connection.QueryFirstAsync<int>(
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

        public async Task DeleteLessonAsync(int id)
        {
            await _connection.ExecuteAsync(
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

        public async Task AddTopicToLessonAsync(int lessonId, int topicId)
        {
            await _connection.ExecuteAsync(
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

            var list = (await _connection.QueryAsync<LessonDto, UserDto, TopicDto, LessonDto>(
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

        public async Task<List<LessonDto>> SelectAllLessonsByTeacherIdAsync(int teacherId)
        {
            var lessonDictionary = new Dictionary<int, LessonDto>();

            var list = (await _connection.QueryAsync<LessonDto, UserDto, TopicDto, CourseDto, LessonDto>(
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
               ))
               .Distinct()
               .ToList();

            return list;
        }

        public async Task<LessonDto> SelectLessonByIdAsync(int id)
        {
            LessonDto result = default;

            await _connection.QueryAsync<LessonDto, UserDto, TopicDto, LessonDto>(
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
            return (await _connection.QueryAsync<StudentLessonDto, UserDto, StudentLessonDto>(
                    _studentLessonSelectByLessonIdProcedure,
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

        public async Task AddStudentToLessonAsync(int lessonId, int studentId)
        {
            await _connection.ExecuteAsync(
                 _studentLessonInsertProcedure,
                  new
                  {
                      lessonId,
                      studentId
                  },
                  commandType: CommandType.StoredProcedure
              );

        }

        public async Task DeleteStudentFromLessonAsync(int lessonId, int studentId)
        {
            await _connection.ExecuteAsync(
                  _studentLessonDeleteProcedure,
                   new
                   {
                       lessonId,
                       studentId
                   },
                   commandType: CommandType.StoredProcedure
               );
        }

        public async Task UpdateStudentFeedbackForLessonAsync(StudentLessonDto studentLessonDto)
        {
            await _connection.ExecuteAsync(
                 _studentLessonUpdateFeedbackProcedure,
                  new
                  {
                      studentLessonDto.Feedback,
                      LessonId = studentLessonDto.Lesson.Id,
                      UserId = studentLessonDto.Student.Id
                  },
                  commandType: CommandType.StoredProcedure
              );
        }

        public async Task UpdateStudentAbsenceReasonOnLessonAsync(StudentLessonDto studentLessonDto)
        {
            await _connection.ExecuteAsync(
                  _studentLessonUpdateAbsenceReasonProcedure,
                   new
                   {
                       studentLessonDto.AbsenceReason,
                       LessonId = studentLessonDto.Lesson.Id,
                       UserId = studentLessonDto.Student.Id
                   },
                   commandType: CommandType.StoredProcedure
               );
        }

        public async Task UpdateStudentAttendanceOnLessonAsync(StudentLessonDto studentLessonDto)
        {
            await _connection.ExecuteAsync(
                 _studentLessonUpdateIsPresentProcedure,
                  new
                  {
                      studentLessonDto.IsPresent,
                      LessonId = studentLessonDto.Lesson.Id,
                      UserId = studentLessonDto.Student.Id
                  },
                  commandType: CommandType.StoredProcedure
              );
        }

        public async Task<List<StudentLessonDto>> SelectAllFeedbackByLessonIdAsync(int lessonId)
        {
            StudentLessonDto result;

            var list = (await _connection.QueryAsync<StudentLessonDto, UserDto, StudentLessonDto>(
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
                  ))
                  .ToList();

            return list;
        }

        public async Task<StudentLessonDto> SelectAttendanceByLessonAndUserIdAsync(int lessonId, int studentId)
        {
            return (await _connection.QueryAsync<StudentLessonDto, LessonDto, UserDto, StudentLessonDto>(
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
                    UserId = studentId
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            )).FirstOrDefault();
        }
    }
}