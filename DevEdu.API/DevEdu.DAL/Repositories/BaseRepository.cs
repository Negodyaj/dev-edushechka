using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected IDbConnection _connection;
        protected const string _connectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;" +
                                                       "User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60;" +
                                                       "Encrypt=False;TrustServerCertificate=False"; //get from json/singleton?
        public BaseRepository()
        {
            _connection = new SqlConnection(_connectionString);
        }
    }
}
