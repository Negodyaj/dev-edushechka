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
            var lessonDictionary = new Dictionary<int, LessonDto>();

            var  list = _connection
                .Query<LessonDto, UserDto, CommentDto, TopicDto, GroupDto, UserDto, LessonDto>(
                    _lessonSelectAllProcedure,
                    (lesson, teacher, comment, topic, group, student) =>
                    {
                        LessonDto lessonDtoEntry;

                        if (!lessonDictionary.TryGetValue(lesson.Id, out lessonDtoEntry))
                        {
                            lessonDtoEntry = lesson;

                            lessonDtoEntry.TeacherDto = teacher;
                            lessonDtoEntry.CommentDtos = new List<CommentDto>();
                            lessonDtoEntry.TopicDtos = new List<TopicDto> ();
                            lessonDtoEntry.GroupDtos = new List<GroupDto> ();
                            lessonDtoEntry.StudentDtos = new List<UserDto> ();
                            lessonDictionary.Add(lesson.Id, lessonDtoEntry);
                        }
                        if (comment != null)
                        {
                            if (!lessonDtoEntry.CommentDtos.Contains(comment))
                            {
                                lessonDtoEntry.CommentDtos.Add(comment);
                            }
                        }
                        if (!lessonDtoEntry.TopicDtos.Contains(topic))
                        {
                            lessonDtoEntry.TopicDtos.Add(topic);
                        }
                        if (!lessonDtoEntry.GroupDtos.Contains(group))
                        {
                            lessonDtoEntry.GroupDtos.Add(group);
                        }
                        if (!lessonDtoEntry.StudentDtos.Contains(student))
                        {
                            lessonDtoEntry.StudentDtos.Add(student);
                        }

                        return lessonDtoEntry;
                    },
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
            return _connection
                .Query<LessonDto, UserDto, CommentDto, TopicDto, GroupDto, UserDto, LessonDto>(
                    _lessonSelectByIdProcedure,
                    (lesson, teacher, comment, topic, group, student)=>
                    {
                        if (result == null)
                        {
                            result = lesson;
                            result.TeacherDto = teacher;
                            if (comment != null)
                            {
                                result.CommentDtos = new List<CommentDto> { comment };
                            }

                            result.TopicDtos = new List<TopicDto> { topic };
                            result.GroupDtos = new List<GroupDto> { group };
                            result.StudentDtos = new List<UserDto> { student };
                        }
                        else
                        {
                            if(comment != null)
                            {
                                if (!result.CommentDtos.Contains(comment))
                                {
                                    result.CommentDtos.Add(comment);
                                }
                            }
                            if (!result.TopicDtos.Contains(topic))
                            {
                                result.TopicDtos.Add(topic);
                            }
                            if (!result.GroupDtos.Contains(group)) {
                                result.GroupDtos.Add(group);
                            }
                            if (!result.StudentDtos.Contains(student)) {
                                result.StudentDtos.Add(student);
                            }
                        }
                        return lesson;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();

            return result;
        }

        public int UpdateLesson(int id, String commentTeacher, DateTime date)
        {
            return _connection.QuerySingleOrDefault<int>(
                _lessonUpdateProcedure,
                new
                {
                    id,
                    commentTeacher,
                    date
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
