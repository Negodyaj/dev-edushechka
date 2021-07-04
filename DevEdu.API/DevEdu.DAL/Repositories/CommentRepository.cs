using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository : BaseRepository, ICommentRepository
    {
        public CommentRepository()
        {
            _insertProcedure = "dbo.Comment_Insert";
            _deleteProcedure = "dbo.Comment_Delete";
            _selectByIdProcedure = "dbo.Comment_SelectById";
            _selectAllProcedure = "dbo.Comment_SelectAllByUserId";
            _updateProcedure = "dbo.Comment_Update";
        }

        public int AddComment(CommentDto commentDto)
        {
            return _connection.QuerySingle<int>(
                _insertProcedure,
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
                _deleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public CommentDto GetComment(int id)
        {
            return _connection.QuerySingleOrDefault<CommentDto>(
                _selectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<CommentDto> GetCommentsByUser(int userId)
        {
            return _connection
                .Query<CommentDto>(
                    _selectAllProcedure,
                    new { userId },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public void UpdateComment(CommentDto commentDto)
        {
            _connection.Execute(
                _updateProcedure,
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