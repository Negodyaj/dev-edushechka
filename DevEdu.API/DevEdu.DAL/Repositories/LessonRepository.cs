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

        public LessonDto SelectLessonWithCommentsAndStudentsById(int id)
        {
            LessonDto lesson = _connection
                .Query<LessonDto, UserDto, TopicDto, LessonDto>(
                    _lessonSelectByIdProcedure,
                    (lesson, teacher, TopicDto) =>
                    {
                        lesson.Teacher = teacher;
                        return lesson;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();

            lesson.Students = SelectStudentsLessonByLessonId(id);

            _connection
                .Query<LessonDto, CommentDto, LessonDto>(
                    _lessonSelectByIdProcedure,
                    (lesson, comment) =>
                    {
                        if(lesson.Comments == null)
                        {
                            lesson.Comments = new List<CommentDto>();
                        }
                        lesson.Comments.Add(comment);
                        return lesson;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();

            return lesson;
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