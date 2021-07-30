using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private const string _groupInsertProcedure = "dbo.Group_Insert";
        private const string _groupDeleteProcedure = "dbo.Group_Delete";
        private const string _groupSelectByIdProcedure = "dbo.Group_SelectById";
        private const string _groupSelectAllProcedure = "dbo.Group_SelectAll";
        private const string _groupUpdateByIdProcedure = "dbo.Group_UpdateById";
        private const string _groupUpdateGroupStatusProcedure = "dbo.Group_UpdateGroupStatus";


        private const string _userGroupInsertProcedure = "dbo.User_Group_Insert";
        private const string _userGroupDeleteProcedure = "dbo.Tag_Delete";
        private const string _insertGroupLesson = "dbo.Group_Lesson_Insert";
        private const string _deleteGroupLesson = "dbo.Group_Lesson_Delete";
        private const string _insertGroupMaterial = "dbo.Group_Material_Insert";
        private const string _deleteGroupMaterial = "dbo.Group_Material_Delete";
        private const string _groupSelectAllByMaterialIdProcedure = "dbo.Group_SelectByMaterialId";

        private const string _taskToGroupAddProcedure = "dbo.Group_Task_Insert";
        private const string _taskFromGroupDeleteProcedure = "dbo.Group_Task_Delete";
        private const string _taskGroupSelectAllByGroupIdProcedure = "dbo.Group_Task_SelectAllByGroupId";
        private const string _taskGroupSelectByIdProcedure = "dbo.Group_Task_SelectById";
        private const string _taskGroupUpdateProcedure = "dbo.Group_Task_Update";

        public GroupRepository() { }

        public int AddGroup(GroupDto groupDto)
        {
            return _connection.QuerySingle<int>
            (
                _groupInsertProcedure,
                new
                {
                    groupDto.Id,
                    groupDto.Name,
                    groupDto.Course
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteGroup(int id)
        {
            _connection.Execute
            (
                _groupDeleteProcedure,
                new { Id = id},
                commandType: CommandType.StoredProcedure
            );
        }

        public GroupDto GetGroup(int id)
        {
            return _connection
                .Query<GroupDto, CourseDto, GroupDto>
            (
                _groupSelectByIdProcedure,
                (group, course) =>
                {
                    GroupDto dto = group;
                    group.Course = course;
                    group.Students = new List<UserDto>();
                    group.Teachers = new List<UserDto>();
                    group.Tutors = new List<UserDto>();
                    return dto;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            )
            .FirstOrDefault();
        }

        public List<GroupDto> GetGroups()
        {
            return _connection
                .Query<GroupDto, CourseDto, GroupDto>
            (
                _groupSelectAllProcedure,
                (group, course) =>
                {
                    GroupDto groupDto;
                    groupDto = group;
                    groupDto.Course = course;
                    return groupDto;
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            )
            .Distinct()
            .ToList();
        }

        public GroupDto UpdateGroup(int id, GroupDto groupDto)
        {
            return _connection.QuerySingle<GroupDto>
            (
                _groupUpdateByIdProcedure,
                new
                {
                    id,
                    groupDto.Name,
                    groupDto.Course,
                    groupDto.GroupStatus,
                    groupDto.StartDate,
                    groupDto.Timetable,
                    groupDto.PaymentPerMonth
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public GroupDto ChangeGroupStatus(int groupId, int statusId)
        {
            return _connection.QuerySingle<GroupDto>
            (
                _groupUpdateGroupStatusProcedure,
                new
                {
                    groupId,
                    statusId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int AddGroupToLesson(int groupId, int lessonId)
        {
            return _connection.Execute(
                _insertGroupLesson,
                new
                {
                    groupId,
                    lessonId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int RemoveGroupFromLesson(int groupId, int lessonId)
        {
           return  _connection.Execute(
                _deleteGroupLesson,
                new
                {
                    groupId,
                    lessonId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int AddGroupMaterialReference(int groupId, int materialId)
        {
            return _connection.Execute(
                _insertGroupMaterial,
                new
                {
                    groupId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int RemoveGroupMaterialReference(int groupId, int materialId)
        {
            return _connection.Execute(
                _deleteGroupMaterial,
                new
                {
                    groupId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int AddUserToGroup(int groupId, int userId, int roleId)
        {
            return _connection.Execute(
                _userGroupInsertProcedure,
                new
                {
                    groupId,
                    userId,
                    roleId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int DeleteUserFromGroup(int userId, int groupId)
        {
            return _connection.Execute(
                _userGroupDeleteProcedure,
                new
                {
                    userId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int AddTaskToGroup(GroupTaskDto groupTaskDto)
        {
            return _connection.QuerySingle<int>(
                _taskToGroupAddProcedure,
                new
                {
                    GroupId = groupTaskDto.Group.Id,
                    TaskId = groupTaskDto.Task.Id,
                    groupTaskDto.StartDate,
                    groupTaskDto.EndDate
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteTaskFromGroup(int groupId, int taskId)
        {
            _connection.Execute(
                _taskFromGroupDeleteProcedure,
                new
                {
                    groupId,
                    taskId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public List<GroupTaskDto> GetTaskGroupByGroupId(int groupId)
        {
            GroupTaskDto result;
            return _connection
                .Query<GroupTaskDto, TaskDto, GroupTaskDto>(
                    _taskGroupSelectAllByGroupIdProcedure,
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

        public GroupTaskDto GetGroupTask(int groupId, int taskId)
        {
            GroupTaskDto result = default;
            return _connection
                .Query<GroupTaskDto, TaskDto, GroupDto, GroupStatus, GroupTaskDto>(
                    _taskGroupSelectByIdProcedure,
                    (groupTask, task, group, groupStatus) =>
                    {
                        result = groupTask;
                        result.Task = task;
                        result.Group = group;
                        result.Group.GroupStatus = groupStatus;
                        return result;
                    },
                    new
                    {
                        groupId,
                        taskId
                    },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .FirstOrDefault();
        }

        public void UpdateGroupTask(GroupTaskDto groupTaskDto)
        {
            _connection.Execute(
                _taskGroupUpdateProcedure,
                new
                {
                    GroupId = groupTaskDto.Group.Id,
                    TaskId = groupTaskDto.Task.Id,
                    groupTaskDto.StartDate,
                    groupTaskDto.EndDate
                },
                commandType: CommandType.StoredProcedure
            );
        }
        public List<GroupDto> GetGroupsByMaterialId(int id)
        {
            return _connection.Query<GroupDto>(
                    _groupSelectAllByMaterialIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }
    }    
}