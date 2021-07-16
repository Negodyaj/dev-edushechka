using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using DevEdu.DAL.Models;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class LessonRepository : BaseRepository, ILessonRepository
    {
        private const string _lessonAddProcedure = "dbo.Lesson_Insert";
        private const string _lessonDeleteProcedure = "dbo.Lesson_Delete";
        private const string _lessonSelectAllByGroupIdProcedure = "dbo.Lesson_SelectAllByGroupId";
        private const string _lessonSelectAllByTeacherIdProcedure = "dbo.Lesson_SelectAllByTeacherId";
        private const string _lessonSelectByIdProcedure = "dbo.Lesson_SelectById";
        private const string _lessonUpdateProcedure = "dbo.Lesson_Update";
        private const string _commentsFromLessonSelectByLessonIdProcedure = "dbo.Lesson_Comments_SelectById";

        private const string _commentAddToLessonProcedure = "dbo.Lesson_Comment_Insert";
        private const string _commentDeleteFromLessonProcedure = "dbo.Lesson_Comment_Delete";
        private const string _topicAddToLessonProcedure = "dbo.Lesson_Topic_Insert";
        private const string _topicDeleteFromLessonProcedure = "dbo.Lesson_Topic_Delete";
        
        private const string _studentLessonInsertProcedure = "dbo.Student_Lesson_Insert";
        private const string _studentLessonDeleteProcedure = "dbo.Student_Lesson_Delete";
        private const string _updateFeedbackProcedure = "dbo.Student_Lesson_UpdateFeedback";
        private const string _updateAbsenceReasonProcedure = "dbo.Student_Lesson_UpdateAbsenceReason";
        private const string _updateIsPresentProcedure = "dbo.Student_Lesson_UpdateIsPresent";

        public LessonRepository()
        {

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

        public int AddLesson(LessonDto lessonDto)
        {
            return _connection.QueryFirst<int>(
               _lessonAddProcedure,
                new
                {
                    Date = lessonDto.Date,
                    TeacherComment = lessonDto.TeacherComment,
                    LinkToRecord = lessonDto.LinkToRecord,
                    TeacherId = lessonDto.Teacher.Id
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

        public void DeleteLesson(int id)
        {
            _connection.Execute(
                _lessonDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<LessonDto> SelectAllLessonsByGroupId(int groupId)
        {
            var lessonDictionary = new Dictionary<int, LessonDto>();

             var list = _connection
                .Query<LessonDto, UserDto, TopicDto, LessonDto>(
                    _lessonSelectAllByGroupIdProcedure,
                    (lesson, teacher, topic) =>
                    {
                        LessonDto lessonEntry;

                        if (!lessonDictionary.TryGetValue(lesson.Id, out lessonEntry))
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
                )
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
                       LessonDto lessonEntry;

                       if (!lessonDictionary.TryGetValue(lesson.Id, out lessonEntry))
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
                   new { teacherId},
                   splitOn: "Id",
                   commandType: CommandType.StoredProcedure
               )
               .Distinct()
               .ToList();

            return list;
        }

        public LessonDto SelectLessonById(int id)
        {
            LessonDto result = new LessonDto();
            _connection
                .Query<LessonDto, UserDto, TopicDto, LessonDto>(
                    _lessonSelectByIdProcedure,
                    (lesson, teacher, topic)=>
                    {
                        result.Teacher = teacher;
                        if(result.Topics == null)
                        {
                            result.Topics = new List<TopicDto>();
                        }
                        result.Topics.Add(topic);
                        return lesson;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();

            return result;
        }

        public LessonDto SelectLessonWithCommentsById(int id)
        {
            LessonDto result = new LessonDto();
            _connection
                .Query<LessonDto, UserDto, TopicDto, LessonDto>(
                    _lessonSelectByIdProcedure,
                    (lesson, teacher, topic) =>
                    {
                        result.Teacher = teacher;
                        if (result.Topics == null)
                        {
                            result.Topics = new List<TopicDto>();
                        }
                        result.Topics.Add(topic);
                        return lesson;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();

            result.Comments = SelectCommentsFromLessonByLessonId(id);

            return result;
        }

        public LessonDto SelectLessonWithCommentsAndStudentsById(int id)
        {
            LessonDto result = SelectLessonWithCommentsById(id);

            result.Students = SelectStudentsLessonByLessonId(id);

            return result;
        }

        public List<CommentDto> SelectCommentsFromLessonByLessonId(int lessonId)
        {
            return _connection
                .Query<CommentDto>(
                    _commentsFromLessonSelectByLessonIdProcedure,
                    new { lessonId },
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }

        //change UserId  to UserDto
        public List<StudentLessonDto> SelectStudentsLessonByLessonId(int lessonId)
        {
            return _connection
                .Query<StudentLessonDto, UserDto, StudentLessonDto>(
                    "[dbo].[Student_Lesson_SelectByLessonId]",
                    (studentLesson, user) =>
                    {
                        studentLesson.UserId = user.Id;
                        return studentLesson;
                    },
                    new { lessonId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();            
        }

        public void UpdateLesson(LessonDto lessonDto)
        {
             _connection.QuerySingleOrDefault<int>(
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
        
        

        public void AddStudentToLesson(int lessonId, int userId)
        {
            _connection.Execute(
            _studentLessonInsertProcedure,
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
            _studentLessonDeleteProcedure,
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
            _updateFeedbackProcedure,
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
            _updateAbsenceReasonProcedure,
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
            _updateIsPresentProcedure,
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