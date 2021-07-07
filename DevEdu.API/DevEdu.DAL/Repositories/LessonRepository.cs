using System;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
                    lessonDto.TeacherId
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
                .Query<LessonDto>(
                    _lessonSelectAllProcedure,
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public LessonDto SelectLessonById(int id)
        {
            return _connection.QuerySingleOrDefault<LessonDto>(
                _lessonSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public int UpdateLesson(int id, String commentDto, DateTime date)
        {
            return _connection.QuerySingleOrDefault<int>(
                _lessonUpdateProcedure,
                new
                {
                    id,
                    commentDto,
                    date
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}
}


