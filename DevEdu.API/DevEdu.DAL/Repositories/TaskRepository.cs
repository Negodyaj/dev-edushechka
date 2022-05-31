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
        private const string _taskSelectByGroupIdProcedure = "dbo.Task_SelectByGroupId";
        private const string _taskSelectAllProcedure = "dbo.Task_SelectAll";
        private const string _taskUpdateProcedure = "dbo.Task_Update";

        public TaskRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddTaskAsync(TaskDto taskDto, int groupId = -1)
        {
            int taskId = await _connection.QuerySingleAsync<int>(
                _taskInsertProcedure,
                new
                {
                    taskDto.Name,
                    taskDto.Description,
                    taskDto.Links,
                    taskDto.IsRequired,
                    groupId
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
            TaskDto result = default;
            return (await _connection
                .QueryAsync<TaskDto, GroupDto, CourseDto, TaskDto>(
                    _taskSelectByIdProcedure,
                    (task, group, course) =>
                    {
                        if (group != default)
                        {
                            task.Group = group;
                        }

                        if (course == null)
                            return task;

                        if (result == null)
                        {
                            result = task;
                            result.Courses = new List<CourseDto> { new CourseDto { Id = course.Id } };
                        }
                        else
                        {
                            result.Courses.Add(new CourseDto { Id = course.Id });
                        }
                        return result;
                    },
                    new { id },
                    commandType: CommandType.StoredProcedure))
                .FirstOrDefault();
        }

        public async Task<List<TaskDto>> GetTasksByCourseIdAsync(int courseId)
        {
            return (await _connection.QueryAsync<TaskDto, GroupDto, TaskDto>(
                    _taskSelectByCourseIdProcedure,
                    (task, group) =>
                    {
                        if (group != default)
                        {
                            task.Group = group;
                        }

                        return task;
                    },
                    new { courseId },
                    commandType: CommandType.StoredProcedure))
                .ToList();
        }

        public async Task<List<TaskDto>> GetTasksByGroupIdAsync(int groupId)
        {
            return (await _connection.QueryAsync<TaskDto, GroupDto, TaskDto>(
                    _taskSelectByGroupIdProcedure,
                    (task, group) =>
                    {
                        if (group != default)
                        {
                            task.Group = group;
                        }

                        return task;
                    },
                    new { groupId },
                    commandType: CommandType.StoredProcedure))
                .ToList();
        }

        public async Task<List<TaskDto>> GetTasksAsync()
        {
            return (await _connection.QueryAsync<TaskDto>(
                    _taskSelectAllProcedure,
                    commandType: CommandType.StoredProcedure))
                .ToList();
        }
    }
}