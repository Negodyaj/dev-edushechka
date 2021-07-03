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
        private const string СonnectionString =
            "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;";
        protected IDbConnection _connection;

        protected BaseRepository()
        {
            _connection = new SqlConnection(СonnectionString);
        }
    }
}
