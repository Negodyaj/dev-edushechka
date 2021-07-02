using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class User_GroupRepository
    {
        private IDbConnection _dbConnection;
        public string ConnectionString { get; set; }
        public User_GroupRepository()
        {
            ConnectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;" +
                                                       "User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60;" +
                                                       "Encrypt=False;TrustServerCertificate=False"; //get from json/singleton?

            _dbConnection = new SqlConnection(ConnectionString);
        }

        public void AddTag(int groupId, int userId, int roleId)
        {
            string query = "exec [dbo].[User_Group_Insert]";

            _dbConnection.Query(query, new { groupId, userId, roleId }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTag(int userId, int groupId)
        {
            string query = "exec [dbo].[Tag_Delete]";

            _dbConnection.Query(query, new { userId, groupId }, commandType: CommandType.StoredProcedure);
        }
    }
}
