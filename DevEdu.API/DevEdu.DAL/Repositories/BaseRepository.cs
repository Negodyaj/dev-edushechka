using System.Data;
using System.Data.SqlClient;

namespace DevEdu.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected const string _connectionString =
          @"Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;";

        protected IDbConnection _connection;

        protected BaseRepository()
        {
            _connection = new SqlConnection(_connectionString);
        }
    }
}