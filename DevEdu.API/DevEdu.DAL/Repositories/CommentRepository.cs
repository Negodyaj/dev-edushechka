using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public int AddComment(CommentDto commentDto)
        {
            return _connection.QuerySingle<int>(
                "dbo.Comment_Insert",
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
                "dbo.Comment_Delete",
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CommentDto GetComment(int id)
        {
            return _connection.QuerySingle<CommentDto>(
                "dbo.Comment_SelectById",
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CommentDto> GetCommentsByUser(int userId)
        {
            return _connection
                .Query<CommentDto>(
                    "dbo.Comment_SelectAllByUserId",
                    new { userId },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateComment(int id, CommentDto commentDto)
        {
            _connection.Execute(
                "dbo.Comment_Update",
                new
                {
                    id,
                    commentDto.Text
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}