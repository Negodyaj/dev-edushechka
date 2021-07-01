using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository
    {
        private static string _connectionString =
            @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DevEdu.Db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        //private static string _connectionString =
        //    @"Data Source=80.78.240.16;Initial Catalog=DevEdu;Persist Security Info=True;User ID=student;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        private IDbConnection _connection = new SqlConnection(_connectionString);

        public int AddComment(CommentDto commentDto)
        {
            return _connection.QuerySingle<int>("dbo.Comment_Insert", new
                {
                    commentDto.UserId,
                    commentDto.Text
                },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteComment(int id)
        {
            _connection.Query("dbo.Comment_Delete", new
            {
                id
            }, 
                commandType: CommandType.StoredProcedure);
        }

        public CommentDto GetComment(int id)
        {
            return _connection.QuerySingle<CommentDto>("dbo.Comment_SelectById", new
            {
                id
            },
                commandType: CommandType.StoredProcedure);
        }

        public List<CommentDto> GetCommentsByUser(int userId)
        {
            return _connection.Query<CommentDto>("dbo.Comment_SelectAllByUserId", new
            {
                userId
            },
                commandType: CommandType.StoredProcedure).AsList();
        }

        public void UpdateComment(int id, CommentDto commentDto)
        {
            _connection.Query("dbo.Comment_Update",new
                {
                    id,
                    commentDto.Text
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}