using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class HomeworkRepository : BaseRepository, IHomeworkRepository
    {
        private const string _homeworkAddProcedure = "dbo.Homework_Insert";
        private const string _homeworkDeleteProcedure = "dbo.Homework_Delete";
        private const string _homeworkSelectAllByGroupIdProcedure = "dbo.Homework_SelectAllByGroupId";
        private const string _homeworkSelectByIdProcedure = "dbo.Homework_SelectById";
        private const string _homeworkUpdateProcedure = "dbo.Homework_Update";
        private const string _homeworkSelectAllByTaskIdProcedure = "dbo.homework_SelectAllByTaskId";

        public HomeworkRepository(){}

        public int AddHomework(HomeworkDto homeworkDto)
        {
            return _connection.QuerySingle<int>(
                _homeworkAddProcedure,
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

        public void DeleteHomework(int id)
        {
            _connection.Execute(
                _homeworkDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            );
        }

        public void UpdateHomework(HomeworkDto homeworkDto)
        {
            _connection.Execute(
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

        public HomeworkDto GetHomework(int id)
        {
            HomeworkDto result = default;
            return _connection
                .Query<HomeworkDto, TaskDto, GroupDto, GroupStatus, HomeworkDto>(
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
                )
                .FirstOrDefault();
        }

        public List<HomeworkDto> GetHomeworkByGroupId(int groupId)
        {
            HomeworkDto result;
            return _connection
                .Query<HomeworkDto, TaskDto, HomeworkDto>(
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
                )
                .ToList();
        }

        public List<HomeworkDto> GetHomeworkByTaskId(int taskId)
        {
            HomeworkDto result;
            return _connection
                .Query<HomeworkDto, GroupDto, GroupStatus, HomeworkDto>(
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
                )
                .ToList();
        }
    }
}