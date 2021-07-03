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
            TaskDto task = _connection.QuerySingle<TaskDto>(
                "dbo.Task_SelectById",
                new { id },
                commandType: CommandType.StoredProcedure
                );
            return task;
        }

        public List<TaskDto> GetTasks()
        {
            List<TaskDto> tasks = _connection.Query<TaskDto>(
                "dbo.Task_SelectAll",
                commandType: CommandType.StoredProcedure
                )
                .ToList<TaskDto>();
            return tasks;
        }

        public int AddTask(TaskDto task)
        {
            int taskId = -1;
            taskId = _connection.QuerySingle<int>(
                "dbo.Task_Insert",
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
            _connection.Execute(
                "dbo.Task_Update",
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
            _connection.Execute(
                "dbo.Task_Delete",
                new { id },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
