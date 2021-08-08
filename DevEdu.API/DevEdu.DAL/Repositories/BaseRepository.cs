using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using System.Data.SqlClient;

namespace DevEdu.DAL.Repositories
{
    public abstract class BaseRepository
    {

        protected IDbConnection _connection;
        protected readonly DatabaseSettings _options;

        protected BaseRepository(IOptions<DatabaseSettings> options)
        {
            var envName = CheckString(options.Value.ConnectionString);

            var сonnectionString = Environment.GetEnvironmentVariable(envName); 
             _connection = new SqlConnection(сonnectionString);
        }
        private string CheckString(string str)
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