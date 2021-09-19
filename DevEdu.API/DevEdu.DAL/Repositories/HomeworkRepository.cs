using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class HomeworkRepository : BaseRepository, IHomeworkRepository
    {
        private const string _homeworkInsertProcedure = "dbo.Homework_Insert";
        private const string _homeworkDeleteProcedure = "dbo.Homework_Delete";
        private const string _homeworkSelectAllByGroupIdProcedure = "dbo.Homework_SelectAllByGroupId";
        private const string _homeworkSelectByIdProcedure = "dbo.Homework_SelectById";
        private const string _homeworkUpdateProcedure = "dbo.Homework_Update";
        private const string _homeworkSelectAllByTaskIdProcedure = "dbo.homework_SelectAllByTaskId";

        public HomeworkRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddHomeworkAsync(HomeworkDto homeworkDto)
        {
            return await _connection.QuerySingleAsync<int>(
                _homeworkInsertProcedure,
                new
                {
                    GroupId = homeworkDto.Group.Id,
                    TaskId = homeworkDto.Task.Id,
                    homeworkDto.StartDate,
                    homeworkDto.EndDate
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteHomeworkAsync(int id)
        {
           await _connection.ExecuteAsync(
                _homeworkDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task UpdateHomeworkAsync(HomeworkDto homeworkDto)
        {
           await _connection.ExecuteAsync(
                _homeworkUpdateProcedure,
                new
                {
                    homeworkDto.Id,
                    homeworkDto.StartDate,
                    homeworkDto.EndDate
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<HomeworkDto> GetHomeworkAsync(int id)
        {
            HomeworkDto result = default;

            return (await _connection
                .QueryAsync<HomeworkDto, TaskDto, GroupDto, GroupStatus, HomeworkDto>(
                _homeworkSelectByIdProcedure,
                (groupTask, task, group, groupStatus) =>
                {
                    result = groupTask;
                    result.Task = task;
                    result.Group = group;
                    result.Group.GroupStatus = groupStatus;
                    
                    return result;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                ))
                .FirstOrDefault();
        }

        public async Task<List<HomeworkDto>> GetHomeworkByGroupIdAsync(int groupId)
        {
            HomeworkDto result = default;

            return (await _connection
                .QueryAsync<HomeworkDto, TaskDto, HomeworkDto>(
                _homeworkSelectAllByGroupIdProcedure,
                (groupTask, task) =>
                {
                    result = groupTask;
                    result.Task = task;
                    
                    return result;
                },
                new { groupId },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task<List<HomeworkDto>> GetHomeworkByTaskIdAsync(int taskId)
        {
            HomeworkDto result = default;

            return (await _connection
                .QueryAsync<HomeworkDto, GroupDto, GroupStatus, HomeworkDto>(
                _homeworkSelectAllByTaskIdProcedure,
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
                ))
                .ToList();
        }
    }
}