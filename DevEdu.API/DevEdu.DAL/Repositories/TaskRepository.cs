using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        private const string _taskInsertProcedure = "dbo.Task_Insert";
        private const string _taskDeleteProcedure = "dbo.Task_Delete";
        private const string _taskSelectByIdProcedure = "dbo.Task_SelectById";
        private const string _taskSelectByCourseIdProcedure = "dbo.Task_SelectByCourseId";
        private const string _taskSelectAllProcedure = "dbo.Task_SelectAll";
        private const string _taskUpdateProcedure = "dbo.Task_Update";

        private const string _tagTaskInsertProcedure = "dbo.Tag_Task_Insert";
        private const string _tagTaskDeleteProcedure = "dbo.Tag_Task_Delete";

        public TaskRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddTaskAsync(TaskDto taskDto)
        {
            int taskId = await _connection.QuerySingleAsync<int>(
                _taskInsertProcedure,
                new
                {
                    taskDto.Name,
                    taskDto.Description,
                    taskDto.Links,
                    taskDto.IsRequired
                },
                commandType: CommandType.StoredProcedure
                );

            return taskId;
        }

        public async Task UpdateTaskAsync(TaskDto taskDto)
        {
            await _connection.ExecuteAsync(
                _taskUpdateProcedure,
                new
                {
                    taskDto.Id,
                    taskDto.Name,
                    taskDto.Description,
                    taskDto.Links,
                    taskDto.IsRequired
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> DeleteTaskAsync(int id)
        {
            return await _connection.ExecuteAsync(
                _taskDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<TaskDto> GetTaskByIdAsync(int id)
        {
            TaskDto task = default;

            return (await _connection
                .QueryAsync<TaskDto, TagDto, TaskDto>(
                    _taskSelectByIdProcedure,
                    (taskDto, tagDto) =>
                    {
                        if (task == default)
                        {
                            task = taskDto;
                            task.Tags = new List<TagDto>();

                        }
                        if (tagDto != null)
                            task.Tags.Add(tagDto);
                        return task;
                    },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure))
                .FirstOrDefault();
        }

        public async Task<List<TaskDto>> GetTasksByCourseIdAsync(int courseId)
        {
            return (await _connection.QueryAsync<TaskDto>(
                    _taskSelectByCourseIdProcedure,
                    new { courseId },
                    commandType: CommandType.StoredProcedure))
                .ToList();
        }

        public async Task<List<TaskDto>> GetTasksAsync()
        {
            var taskDictionary = new Dictionary<int, TaskDto>();

            var list = (await _connection.QueryAsync<TaskDto, TagDto, TaskDto>(
                    _taskSelectAllProcedure,
                (taskDto, tagDto) =>
                {
                    if (!taskDictionary.TryGetValue(taskDto.Id, out var taskDtoEntry))
                    {
                        taskDtoEntry = taskDto;
                        taskDtoEntry.Tags = new List<TagDto>();
                        taskDictionary.Add(taskDtoEntry.Id, taskDtoEntry);
                    }
                    taskDtoEntry.Tags.Add(tagDto);

                    return taskDtoEntry;
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure))
                .Distinct()
                .ToList();

            return list;
        }

        public async Task<int> AddTagToTaskAsync(int taskId, int tagId)
        {
            return await _connection
                .ExecuteAsync(_tagTaskInsertProcedure,
                new { tagId, taskId },
                commandType: CommandType.StoredProcedure
                );
        }

        public async Task<int> DeleteTagFromTaskAsync(int taskId, int tagId)
        {
            return await _connection
                .ExecuteAsync(_tagTaskDeleteProcedure,
                new { tagId, taskId },
                commandType: CommandType.StoredProcedure
                );
        }
    }
}