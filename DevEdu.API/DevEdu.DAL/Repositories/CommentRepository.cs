using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class CommentRepository
    {
        private string _connection =
            @"Data Source=(localdb)\ProjectsV13;Initial Catalog=DevEdu.Db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        //private string _connection =
        //    @"Data Source=80.78.240.16;Initial Catalog=DevEdu;Persist Security Info=True;User ID=student;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";

        public int AddComment(CommentDto commentDto)
        {
            int id;
            string query = "exec dbo.Comment_Insert ";
            string value = $"N'{commentDto.UserId}', " +
                           $"N'{commentDto.Text}'";
            using (IDbConnection connection = new SqlConnection(_connection))
            {
                id = connection.QueryFirst<int>(@$"{query}{value}");
            }
            return id;
        }

        public void DeleteComment(int id)
        {
            string query = $"exec dbo.Comment_Delete {id}";
            using (IDbConnection connection = new SqlConnection(_connection))
            {
                connection.Query(@$"{query}");
            }
        }

        public CommentDto GetComment(int id)
        {
            CommentDto commentDto;
            string query = $"exec dbo.Comment_SelectById {id}";
            using (IDbConnection connection = new SqlConnection(_connection))
            {
                commentDto=connection.QueryFirst<CommentDto>(@$"{query}");
            }
            return commentDto;
        }

        public List<CommentDto> GetCommentsByUser(int userId)
        {
            List<CommentDto> commentDtos;
            string query = $"exec dbo.Comment_SelectAllByUserId {userId}";
            using (IDbConnection connection = new SqlConnection(_connection))
            {
                commentDtos = connection.Query<CommentDto>(@$"{query}").AsList();
            }
            return commentDtos;
        }

        public void UpdateComment(int id, CommentDto commentDto)
        {
            string query = $"exec dbo.Comment_Update {id}";
            string value = $"N'{commentDto.Text}'";
            using (IDbConnection connection = new SqlConnection(_connection))
            {
                connection.Query(@$"{query}{value}");
            }
        }
    }
}