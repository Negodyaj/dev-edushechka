using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        private const string _commentInsertProcedure = "dbo.Comment_Insert";
        private const string _commentDeleteProcedure = "dbo.Comment_Delete";
        private const string _commentSelectByIdProcedure = "dbo.Comment_SelectById";
        private const string _commentSelectAllByUserProcedure = "dbo.Comment_SelectAllByUserId";
        private const string _commentUpdateProcedure = "dbo.Comment_Update";
        public CommentRepository() { }

        public int AddComment(CommentDto commentDto)
        {
            return _connection.QuerySingle<int>(
                _commentInsertProcedure,
                new
                {
                    commentDto.UserId,
                    commentDto.Text
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
            return _connection.QuerySingleOrDefault<CommentDto>(
                _commentSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CommentDto> GetCommentsByUser(int userId)
        {
            return _connection
                .Query<CommentDto>(
                    _commentSelectAllByUserProcedure,
                    new { userId },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateComment(CommentDto commentDto)
        {
            _connection.Execute(
                _commentUpdateProcedure,
                new
                {
                    commentDto.Id,
                    commentDto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}