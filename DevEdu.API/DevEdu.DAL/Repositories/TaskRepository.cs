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
        private static string _connectionString =
            "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23;" +
            " Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";
        private IDbConnection _connection = new SqlConnection(_connectionString);

        public TaskRepository()
        {

        }
        public TaskDto GetTaskById(int id)
        {
            TaskDto task = new TaskDto();
            string query; 
            task = _connection.QuerySingle<TaskDto>("dbo.Task_SelectById", new { id }, commandType: CommandType.StoredProcedure);
            return task;
        }

        public List<TaskDto> GetTasks()
        {
            List<TaskDto> tasks = new List<TaskDto>();
            string query;
            tasks = _connection.Query<TaskDto>("dbo.Task_SelectAll", commandType: CommandType.StoredProcedure).ToList<TaskDto>();
            return tasks;
        }

        public int AddTask(TaskDto task)
        {
            int taskId = -1;
            taskId = _connection.QuerySingle<int>("dbo.Task_Insert", new { task.Name, task.StartDate, task.EndDate, task.Description, task.Links, task.IsRequired }, commandType: CommandType.StoredProcedure);
            return taskId;
        }

        public int UpdateTask(TaskDto task)
        {
            _connection.QuerySingleOrDefault<int>("dbo.Task_Update", new { task.Id, task.Name, task.StartDate, task.EndDate, task.Description, task.Links, task.IsRequired }, commandType: CommandType.StoredProcedure);
            return task.Id;
        }

        public int DeleteTask(int id)
        {
            _connection.QuerySingleOrDefault<int>("dbo.Task_Delete", new { id }, commandType: CommandType.StoredProcedure);
            return id;
        }
    }
}
