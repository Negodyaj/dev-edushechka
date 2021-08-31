using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.Data;
using System.Linq;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        private const string _commentAddProcedure = "dbo.Comment_Insert";
        private const string _commentDeleteProcedure = "dbo.Comment_Delete";
        private const string _commentSelectByIdProcedure = "dbo.Comment_SelectById";
        private const string _commentUpdateProcedure = "dbo.Comment_Update";
        private const string _commentsFromLessonSelectByLessonIdProcedure = "dbo.Comment_SelectByLessonId";

        public CommentRepository(IOptions<DatabaseSettings> options) : base(options){ }

        public int AddComment(CommentDto dto)
        {
            return _connection.QuerySingle<int>(
                _commentAddProcedure,
                new
                {
                    userId = dto.User.Id,
                    lessonId = dto.Lesson == null ? null : (int?)dto.Lesson.Id,
                    studentHomeworkId = dto.StudentHomework == null ? null : (int?)dto.StudentHomework.Id,
                    dto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteComment(int id)
        {
            _connection.Execute(
                _commentDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CommentDto GetComment(int id)
        {
            CommentDto result = default;
            return _connection
                .Query<CommentDto, UserDto, Role, LessonDto, StudentHomeworkDto, CommentDto>(
                    _commentSelectByIdProcedure,
                    (comment, user, role, lesson, studentHomework) =>
                    {
                        if (result == null)
                        {
                            result = comment;
                            result.User = user;
                            result.User.Roles = new List<Role> { role };
                            result.Lesson = lesson;
                            result.StudentHomework = studentHomework;
                        }
                        else
                        {
                            result.User.Roles.Add(role);
                        }
                        return result;
                    },
                    new { id },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public void UpdateComment(CommentDto dto)
        {
            _connection.Execute(
                _commentUpdateProcedure,
                new
                {
                    dto.Id,
                    dto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CommentDto> SelectCommentsFromLessonByLessonId(int lessonId)
        {
            CommentDto result = default;
            return _connection
                .Query<CommentDto, UserDto, CommentDto>(
                    _commentsFromLessonSelectByLessonIdProcedure,
                    (comment, user) =>
                    {
                        result = comment;
                        result.User = user;
                        return result;
                    },
                    new { lessonId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .Distinct()
                .ToList();
        }
    }
}