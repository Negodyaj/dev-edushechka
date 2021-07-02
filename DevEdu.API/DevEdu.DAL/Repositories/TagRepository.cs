using Dapper;
using DevEdu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class TagRepository
    {
        private IDbConnection _dbConnection;
        public string ConnectionString { get; set; }
        public TagRepository()
        {
            ConnectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;" +
                                                       "User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60;" +
                                                       "Encrypt=False;TrustServerCertificate=False"; //get from json/singleton?

            _dbConnection = new SqlConnection(ConnectionString);
        }

        public int AddTag(TagDto tagDto)
        {
            string query = "exec [dbo].[Tag_Insert]";

            return _dbConnection.QuerySingle<int>(query, new { tagDto.Name }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTag(int id)
        {
            string query = "exec [dbo].[Tag_Delete]";

            _dbConnection.Query(query, new { id }, commandType: CommandType.StoredProcedure);
        }
        public List<TagDto> SelectAllTags()
        {
            string query = "exec [dbo].[Tag_SelectAll]";

            List<TagDto> tagDtos = _dbConnection.Query<TagDto>(query).ToList();

            return tagDtos;
        }

        public TagDto SelectTagById(int id)
        {
            string query = "exec [dbo].[Tag_SelectByID]";

            TagDto tagDto = _dbConnection.QuerySingle<TagDto>(query, new { id }, commandType: CommandType.StoredProcedure);

            return tagDto;
        }
        public void UpdateTag(TagDto tagDto)
        {
            string query = "exec [dbo].[Tag_Update]";

            _dbConnection.Query(query, new { tagDto.Id, tagDto.Name }, commandType: CommandType.StoredProcedure);
        }
    }
}
