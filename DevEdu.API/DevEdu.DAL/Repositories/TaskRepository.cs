using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        private const string _tagTaskAddProcedure = "dbo.Tag_Task_Insert";
        private const string _tagTaskDeleteProcedure = "dbo.Tag_Task_Delete";

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

        public int AddTask(TaskDto task)
        {
            int taskId = _connection.QuerySingle<int>(
                _taskAddProcedure,
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
                _taskUpdateProcedure,
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
                _taskDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
                );
        }

        public int AddTagToTagTask(int taskId, int tagId)
        {
            return _connection
                .QuerySingle(_tagTaskAddProcedure,
                new { tagId, taskId },
                commandType: CommandType.StoredProcedure
                );
        }

        public void DeleteTagFromTask(int taskId, int tagId)
        {
            _connection
                .Execute(_tagTaskDeleteProcedure,
                new { tagId, taskId },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}