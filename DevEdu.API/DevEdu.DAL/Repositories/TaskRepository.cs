using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class TaskRepository
    {
        private readonly string connectionString = "Data Source=80.78.240.16;Persist Security Info=True;User ID = student; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";

        public TaskRepository()
        {

        }
        public List<TaskDto> GetTasks()
        {
            List<TaskDto> result = new List<TaskDto>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                query = "exec dbo.Task_SelectAll";
                result = dbConnection.Query<TaskDto>(query).AsList<TaskDto>();
            }
            return result;
        }
    }
}
