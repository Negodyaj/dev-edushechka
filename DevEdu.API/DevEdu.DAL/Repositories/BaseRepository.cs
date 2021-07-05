using System.Data;
using System.Data.SqlClient;

namespace DevEdu.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected const string ConnectionString =
            @"Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;";

        protected IDbConnection _connection;
        protected string _insertProcedure;
        protected string _deleteProcedure;
        protected string _selectByIdProcedure;
        protected string _selectAllProcedure;
        protected string _updateProcedure;

        protected BaseRepository()
        {
            _connection = new SqlConnection(ConnectionString);
        }
    }
}