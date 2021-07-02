using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class CourseRepository
    {
        private static string _connectionString =
            "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;" +
            " Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";
        private IDbConnection _connection = new SqlConnection(_connectionString);

        public CourseRepository()
        {
                
        }

        public void AddTagToTopic(int topicId, int tagId)
        {
            _connection.Query("dbo.Tag_Topic_Insert", new 
                {
                    topicId, 
                    tagId
                },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteTagFromTopic(int topicId, int tagId)
        {
            _connection.Query("dbo.Tag_Topic_Delete", new
                {
                    topicId,
                    tagId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
