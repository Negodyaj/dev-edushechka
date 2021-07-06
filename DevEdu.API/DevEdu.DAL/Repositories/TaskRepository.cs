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
        private const string _taskAddProcedure = "dbo.Task_Insert";
        private const string _taskDeleteProcedure = "dbo.Task_Delete";
        private const string _taskSelectByIdProcedure = "dbo.Task_SelectById";
        private const string _taskSelectAlldProcedure = "dbo.Task_SelectAll";
        private const string _taskUpdateProcedure = "dbo.Task_Update";

        public TaskRepository()
        {

        }
        public TaskDto GetTaskById(int id)
        {
            TaskDto task = _connection.QuerySingleOrDefault<TaskDto>(
                _taskSelectByIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
                );
            return task;
        }

        public List<TaskDto> GetTasks()
        {
            List<TaskDto> tasks = _connection.Query<TaskDto>(
                    _taskSelectAlldProcedure,
                commandType: CommandType.StoredProcedure
                )
                .ToList<TaskDto>();
            return tasks;
        }

        public int AddTask(TaskDto taskDto)
        {
            int taskId = _connection.QuerySingle<int>(
                _taskAddProcedure,
                new
                {
                    taskDto.Name,
                    taskDto.StartDate,
                    taskDto.EndDate,
                    taskDto.Description,
                    taskDto.Links,
                    taskDto.IsRequired
                },
                commandType: CommandType.StoredProcedure
                );
            return taskId;
        }

        public void UpdateTask(TaskDto taskDto)
        {
            _connection.Execute(
                _taskUpdateProcedure,
                new
                {
                    taskDto.Id,
                    taskDto.Name,
                    taskDto.StartDate,
                    taskDto.EndDate,
                    taskDto.Description,
                    taskDto.Links,
                    taskDto.IsRequired
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteTask(int id)
        {
            _connection.Execute(
                _taskDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}
