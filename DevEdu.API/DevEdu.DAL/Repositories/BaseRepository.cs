using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace DevEdu.DAL.Repositories
{
    public abstract class BaseRepository
    {
        //protected const string _connectionString =
        //    @"Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;";

        protected IDbConnection _connection;
        protected readonly DatabaseSettings _options;

        //protected BaseRepository(string connectionString)
        //{
        //    _connection = new SqlConnection(connectionString);
        //}
        protected BaseRepository(IOptions<DatabaseSettings> options)
        {
            
            var envName = CheackString(options.Value.ConnectionString);

            var сonnectionString = Environment.GetEnvironmentVariable(envName); // null
            //_options = options.Value;
            _connection = new SqlConnection(сonnectionString);
        }
        public string CheackString(string str)
        {
            string result = str;
            if (str.Contains("{{") && str.Contains("}}"))
            {
                result = str.Replace("{{", "").Replace("}}", "");
            }
            return result;
        }
    }
}