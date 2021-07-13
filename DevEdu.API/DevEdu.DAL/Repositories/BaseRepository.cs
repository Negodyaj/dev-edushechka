using System.Data;
using System.Data.SqlClient;

namespace DevEdu.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected const string _connectionString =
          // @"Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;";
          @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=localTest;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False";
        protected IDbConnection _connection;

        protected BaseRepository()
        {
            _connection = new SqlConnection(_connectionString);
        }
    }
}