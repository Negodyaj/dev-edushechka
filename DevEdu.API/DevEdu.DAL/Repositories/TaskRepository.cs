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
        public string connectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";

        public TaskRepository()
        {

        }
        public TaskDto GetTaskById(int id)
        {
            TaskDto task = new TaskDto();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                query = "exec dbo.Task_SelectById @Id";
                task = dbConnection.Query<TaskDto>(query, new { id }).FirstOrDefault();
            }
            return task;
        }

        public List<TaskDto> GetTasks()
        {
            List<TaskDto> tasks = new List<TaskDto>();
            string query;
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                query = "exec dbo.Task_SelectAll";
                tasks = dbConnection.Query<TaskDto>(query).AsList<TaskDto>();
            }
            return tasks;
        }
    }
}
