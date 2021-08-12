using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;

namespace DevEdu.DAL.Repositories
{
    public class TaskRepository : BaseRepository, ITaskRepository
    {
        private const string _taskAddProcedure = "dbo.Task_Insert";
        private const string _taskDeleteProcedure = "dbo.Task_Delete";
        private const string _taskSelectByIdProcedure = "dbo.Task_SelectById";
        private const string _taskSelectByCourseIdProcedure = "dbo.Task_SelectByCourseId";
        private const string _taskSelectAlldProcedure = "dbo.Task_SelectAll";
        private const string _taskUpdateProcedure = "dbo.Task_Update";
        private const string _tagTaskAddProcedure = "dbo.Tag_Task_Insert";
        private const string _tagTaskDeleteProcedure = "dbo.Tag_Task_Delete";
        private const string _taskGroupSelectAllByTaskIdProcedure = "dbo.Group_Task_SelectAllByTaskId";

        public TaskRepository(IOptions<DatabaseSettings> options) : base(options)
        {

        }

        public TaskDto GetTaskById(int id)
        {
            TaskDto task = default;
            _connection.Query<TaskDto, TagDto, TaskDto>(
                _taskSelectByIdProcedure,
                (taskDto, TagDto) =>
                {
                    if (task == null)
                    {
                        task = taskDto;
                        task.Tags = new List<TagDto> { TagDto };
                    }
                    else
                    {
                        task.Tags.Add(TagDto);
                    }

                    return taskDto;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
            return task;
        }
        public List<TaskDto> GetTaskByCourseId(int courseId)
        {
            var taskList = new List<TaskDto>();
            return _connection.Query<TaskDto>(
                    _taskSelectByCourseIdProcedure,
                    new { courseId },
                    commandType: CommandType.StoredProcedure)
                .ToList();
        }
        public List<TaskDto> GetTasks()
        {
            var taskDictionary = new Dictionary<int, TaskDto>();

            var list = _connection.Query<TaskDto, TagDto, TaskDto>(
                    _taskSelectAlldProcedure,
                (taskDto, TagDto) =>
                {
                    TaskDto taskDtoEntry;

                    if (!taskDictionary.TryGetValue(taskDto.Id, out taskDtoEntry))
                    {
                        taskDtoEntry = taskDto;
                        taskDtoEntry.Tags = new List<TagDto>();
                        taskDictionary.Add(taskDtoEntry.Id, taskDtoEntry);
                    }
                    taskDtoEntry.Tags.Add(TagDto);
                    return taskDtoEntry;
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure)
                .Distinct()
                .ToList<TaskDto>();
            return list;
        }

        public int AddTask(TaskDto taskDto)
        {
            int taskId = _connection.QuerySingle<int>(
                _taskAddProcedure,
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

        public void UpdateTask(TaskDto taskDto)
        {
            _connection.Execute(
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

        public void DeleteTask(int id)
        {
            _connection.Execute(
                _taskDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
                );
        }

        public int AddTagToTask(int taskId, int tagId)
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
        public List<GroupTaskDto> GetGroupsByTaskId(int taskId)
        {
            GroupTaskDto result;
            return _connection
                .Query<GroupTaskDto, GroupDto, GroupStatus, GroupTaskDto>(
                    _taskGroupSelectAllByTaskIdProcedure,
                    (groupTask, group, groupStatus) =>
                    {
                        result = groupTask;
                        result.Group = group;
                        result.Group.GroupStatus = groupStatus;
                        return result;
                    },
                    new { taskId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }
    }
}