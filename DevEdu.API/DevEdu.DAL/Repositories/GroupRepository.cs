using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

        private const string _groupSelectPresentGroupForStudentByUserId = "dbo.Group_SelectPresentGroupForStudentByUserId";

        public async Task<int> AddGroup(GroupDto groupDto)
        {
            return await _connection.QuerySingleAsync<int>
            (
                _groupInsertProcedure,
                new
                {
                    groupDto.Name,
                    groupDto.Course
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteGroup(int id)
        {
            await _connection.ExecuteAsync
            (
                _groupDeleteProcedure,
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<GroupDto> GetGroup(int id)
        {
            var answer = await _connection
            .QueryAsync<GroupDto, CourseDto, GroupDto>
            (
                _groupSelectByIdProcedure,
                (group, course) =>
                {
                    GroupDto dto = group;
                    group.Course = course;
                    group.Students = new List<UserDto>();
                    return dto;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            );
            return answer.FirstOrDefault();
        }

        public async Task<List<GroupDto>> GetGroups()
        {
            return (List<GroupDto>)await _connection
            .QueryAsync<GroupDto, CourseDto, GroupDto>
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
            );
        }

        public async Task<GroupDto> UpdateGroup(GroupDto groupDto)
        {

            return await _connection
            .QuerySingleAsync<GroupDto>
            (
                _groupUpdateByIdProcedure,
                new
                {
                    Id = groupDto.Id,
                    Name = groupDto.Name,
                    CourseId = groupDto.Course.Id,
                    GroupStatusId = (int)groupDto.GroupStatus,
                    StartDate = groupDto.StartDate,
                    Timetable = groupDto.Timetable,
                    PaymentPerMonth = groupDto.PaymentPerMonth
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<GroupDto> ChangeGroupStatus(int groupId, int statusId)
        {
            return await _connection
            .QuerySingleAsync<GroupDto>
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

        public async Task<int> AddGroupToLesson(int groupId, int lessonId)
        {
            return await _connection.ExecuteAsync
            (
                _insertGroupLesson,
                new
                {
                    groupId,
                    lessonId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RemoveGroupFromLesson(int groupId, int lessonId)
        {
            await _connection.ExecuteAsync
            (
                 _deleteGroupLesson,
                 new
                 {
                     groupId,
                     lessonId
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task<int> AddGroupMaterialReference(int groupId, int materialId)
        {
            return await _connection.ExecuteAsync
            (
                _insertGroupMaterial,
                new
                {
                    groupId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> RemoveGroupMaterialReference(int groupId, int materialId)
        {
            return await _connection.ExecuteAsync
            (
                _deleteGroupMaterial,
                new
                {
                    groupId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddUserToGroup(int groupId, int userId, int roleId)
        {
            return await _connection.ExecuteAsync
            (
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

        public async Task<int> DeleteUserFromGroup(int userId, int groupId)
        {
            return await _connection.ExecuteAsync
            (
                _userGroupDeleteProcedure,
                new
                {
                    userId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddTaskToGroup(GroupTaskDto groupTaskDto)
        {
            return await _connection
            .QuerySingleAsync<int>
            (
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

        public async Task DeleteTaskFromGroup(int groupId, int taskId)
        {
            await _connection.ExecuteAsync
            (
                _taskFromGroupDeleteProcedure,
                new
                {
                    groupId,
                    taskId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<GroupTaskDto>> GetTaskGroupByGroupId(int groupId)
        {
            GroupTaskDto result;
            return (List<GroupTaskDto>)await _connection
            .QueryAsync<GroupTaskDto, TaskDto, GroupTaskDto>
            (
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
            );
        }

        public async Task<GroupTaskDto> GetGroupTask(int groupId, int taskId)
        {
            GroupTaskDto result = default;
            var answer = await _connection
            .QueryAsync<GroupTaskDto, TaskDto, GroupDto, GroupStatus, GroupTaskDto>
            (
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
            );
            return answer.FirstOrDefault();
        }

        public async Task UpdateGroupTask(GroupTaskDto groupTaskDto)
        {
            await _connection.ExecuteAsync
            (
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
            return _connection.Query<GroupDto>
            (
                _groupSelectAllByMaterialIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        public async Task<int> GetPresentGroupForStudentByUserId(int userId)
        {
            return await _connection.QuerySingleAsync<int>
            (
                _groupSelectPresentGroupForStudentByUserId,
                new { Id = userId },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}