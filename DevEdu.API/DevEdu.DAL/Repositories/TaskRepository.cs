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
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        public TaskRepository()
        {

        }
        public TaskDto GetTaskById(int id)
        {
            string query = "dbo.Task_SelectById";
            TaskDto task = _connection.QuerySingleOrDefault<TaskDto>(
                query,
                new { id },
                commandType: CommandType.StoredProcedure
                );
            return task;
        }

        public List<TaskDto> GetTasks()
        {
            string query = "dbo.Task_SelectAll";
            List<TaskDto> tasks = _connection.Query<TaskDto>(
                query,
                commandType: CommandType.StoredProcedure
                )
                .ToList<TaskDto>();
            return tasks;
        }

        public int AddTask(TaskDto task)
        {
            string query = "dbo.Task_Insert";
            int taskId = _connection.QuerySingle<int>(
                query,
                new
                {
                    task.Name,
                    task.StartDate,
                    task.EndDate,
                    task.Description,
                    task.Links,
                    task.IsRequired
                },
                commandType: CommandType.StoredProcedure
                );
            return taskId;
        }

        public void UpdateTask(TaskDto task)
        {
            string query = "dbo.Task_Update";
            _connection.Execute(
                query,
                new
                {
                    task.Id,
                    task.Name,
                    task.StartDate,
                    task.EndDate,
                    task.Description,
                    task.Links,
                    task.IsRequired
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteTask(int id)
        {
            string query = "dbo.Task_Delete";
            _connection.Execute(
                query,
                new { id },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
