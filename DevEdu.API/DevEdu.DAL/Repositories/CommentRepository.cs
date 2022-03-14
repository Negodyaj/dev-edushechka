using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        private const string _commentInsertProcedure = "dbo.Comment_Insert";
        private const string _commentDeleteProcedure = "dbo.Comment_Delete";
        private const string _commentSelectByIdProcedure = "dbo.Comment_SelectById";
        private const string _commentUpdateProcedure = "dbo.Comment_Update";
        private const string _commentsToLessonSelectByLessonIdProcedure = "dbo.Comment_SelectByLessonId";

        public CommentRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddCommentAsync(CommentDto dto)
        {
            return await _connection.QuerySingleAsync<int>(
                _commentInsertProcedure,
                new
                {
                    userId = dto.User.Id,
                    studentHomeworkId = dto.StudentHomework == null ? null : (int?)dto.StudentHomework.Id,
                    dto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _connection.ExecuteAsync(
                 _commentDeleteProcedure,
                 new { id },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task<CommentDto> GetCommentAsync(int id)
        {
            CommentDto result = default;

            return (await _connection
                .QueryAsync<CommentDto, UserDto, Role, LessonDto, StudentHomeworkDto, CommentDto>(
                _commentSelectByIdProcedure,
                (comment, user, role, lesson, studentHomework) =>
                {
                    if (result == null)
                    {
                        result = comment;
                        result.User = user;
                        result.User.Roles = new List<Role> { role };
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
                ))
                .FirstOrDefault();
        }

        public async Task UpdateCommentAsync(CommentDto dto)
        {
            await _connection.ExecuteAsync(
                 _commentUpdateProcedure,
                 new
                 {
                     dto.Id,
                     dto.Text
                 },
                 commandType: CommandType.StoredProcedure
             );
        }
    }
}