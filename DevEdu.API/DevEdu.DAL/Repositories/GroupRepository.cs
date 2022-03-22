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
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private const string _groupInsertProcedure = "dbo.Group_Insert";
        private const string _groupDeleteProcedure = "dbo.Group_Delete";
        private const string _groupSelectByIdProcedure = "dbo.Group_SelectById";
        private const string _groupSelectAllProcedure = "dbo.Group_SelectAll";
        private const string _groupUpdateByIdProcedure = "dbo.Group_UpdateById";
        private const string _groupUpdateGroupStatusProcedure = "dbo.Group_UpdateGroupStatus";
        private const string _groupSelectAllByMaterialIdProcedure = "dbo.Group_SelectByMaterialId";
        private const string _groupSelectByCourseProcedure = "dbo.Group_SelectByCourseId";
        private const string _groupSelectAllByTaskIdProcedure = "dbo.Group_SelectAllByTaskId";
        private const string _groupSelectGroupsByUserIdProcedure = "dbo.Group_SelectAllByUserId";
        private const string _groupSelectGroupsByLessonIdProcedure = "dbo.Group_SelectAllByLessonId";
        private const string _groupSelectPresentGroupForStudentByUserIdProcedure = "dbo.Group_SelectPresentGroupForStudentByUserId";

        private const string _groupLessonInsertProcedure = "dbo.Group_Lesson_Insert";
        private const string _groupLessonDeleteProcedure = "dbo.Group_Lesson_Delete";

        private const string _groupMaterialInsertProcedure = "dbo.Group_Material_Insert";
        private const string _groupMaterialDeleteProcedure = "dbo.Group_Material_Delete";

        private const string _groupTaskDeleteProcedure = "dbo.Group_Task_Delete";

        private const string _userGroupInsertProcedure = "dbo.User_Group_Insert";
        private const string _userGroupDeleteProcedure = "dbo.User_Group_Delete";

        public GroupRepository(IOptions<DatabaseSettings> options) : base(options)
        {
        }

        public async Task<int> AddGroupAsync(GroupDto groupDto)
        {
            return await _connection.QuerySingleAsync<int>(
                _groupInsertProcedure,
                new
                {
                    groupDto.Name,
                    CourseId = groupDto.Course.Id,
                    groupDto.StartDate,
                    groupDto.EndDate,
                    groupDto.Timetable,
                    groupDto.PaymentPerMonth,
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task DeleteGroupAsync(int id)
        {
            await _connection.ExecuteAsync
            (
                _groupDeleteProcedure,
                new { Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<GroupDto> GetGroupAsync(int id)
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

        public async Task<List<GroupDto>> GetGroupsAsync()
        {
            return (await _connection
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
            ))
            .ToList();
        }

        public async Task<GroupDto> UpdateGroupAsync(GroupDto groupDto)
        {
            return await _connection
            .QuerySingleAsync<GroupDto>
            (
                _groupUpdateByIdProcedure,
                new
                {
                    groupDto.Id,
                    groupDto.Name,
                    CourseId = groupDto.Course.Id,
                    GroupStatusId = (int)groupDto.GroupStatus,
                    groupDto.StartDate,
                    groupDto.EndDate,
                    groupDto.Timetable,
                    groupDto.PaymentPerMonth
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<GroupDto> ChangeGroupStatusAsync(int groupId, int statusId)
        {
            return await _connection
                .QuerySingleAsync<GroupDto>
                (
                    _groupUpdateGroupStatusProcedure,                    
                    new
                    {
                        GroupId = groupId,
                        StatusId = statusId
                    },
                    commandType: CommandType.StoredProcedure
                );
        }

        public async Task<int> AddGroupToLessonAsync(int groupId, int lessonId)
        {
            return await _connection.ExecuteAsync
            (
                _groupLessonInsertProcedure,
                new
                {
                    groupId,
                    lessonId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task RemoveGroupFromLessonAsync(int groupId, int lessonId)
        {
            await _connection.ExecuteAsync
            (
                 _groupLessonDeleteProcedure,
                 new
                 {
                     groupId,
                     lessonId
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public async Task<int> AddGroupMaterialReferenceAsync(int groupId, int materialId)
        {
            return await _connection.ExecuteAsync
            (
                _groupMaterialInsertProcedure,
                new
                {
                    groupId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> RemoveGroupMaterialReferenceAsync(int groupId, int materialId)
        {
            return await _connection.ExecuteAsync
            (
                _groupMaterialDeleteProcedure,
                new
                {
                    groupId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> AddUserToGroupAsync(int groupId, int userId, int roleId)
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

        public async Task<int> DeleteUserFromGroupAsync(int userId, int groupId)
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

        public async Task DeleteTaskFromGroupAsync(int groupId, int taskId)
        {
            await _connection.ExecuteAsync
            (
                _groupTaskDeleteProcedure,
                new
                {
                    groupId,
                    taskId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<GroupDto>> GetGroupsByMaterialIdAsync(int id)
        {
            return (await _connection.QueryAsync<GroupDto>
                (
                _groupSelectAllByMaterialIdProcedure,
                new { id },
                commandType: CommandType.StoredProcedure
            )).ToList();
        }

        public async Task<int> GetPresentGroupForStudentByUserIdAsync(int userId)
        {
            return await _connection.QuerySingleAsync<int>
                (
                _groupSelectPresentGroupForStudentByUserIdProcedure,
                new { Id = userId },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<List<GroupDto>> GetGroupsByCourseIdAsync(int courseId)
        {
            return (await _connection.QueryAsync<GroupDto>
                (
                _groupSelectByCourseProcedure,
                new { courseId },
                commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task<List<GroupDto>> GetGroupsByTaskIdAsync(int taskId)
        {
            GroupDto result;

            return (await _connection
                .QueryAsync<GroupDto, GroupStatus, GroupDto>(
                    _groupSelectAllByTaskIdProcedure,
                    (group, groupStatus) =>
                    {
                        result = group;
                        result.GroupStatus = groupStatus;
                        return result;
                    },
                    new { taskId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task<List<GroupDto>> GetGroupsByLessonIdAsync(int lessonId)
        {
            GroupDto result;

            return (await _connection
                .QueryAsync<GroupDto, GroupStatus, GroupDto>(
                    _groupSelectGroupsByLessonIdProcedure,
                    (group, groupStatus) =>
                    {
                        result = group;
                        result.GroupStatus = groupStatus;
                        return result;
                    },
                    new { lessonId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }

        public async Task<List<GroupDto>> GetGroupsByUserIdAsync(int userId)
        {
            GroupDto result;

            return (await _connection
                .QueryAsync<GroupDto, GroupStatus, CourseDto, GroupDto>(
                    _groupSelectGroupsByUserIdProcedure,
                    (group, groupStatus, course) =>
                    {
                        result = group;
                        result.GroupStatus = groupStatus;
                        result.Course = course;
                        return result;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                ))
                .ToList();
        }
    }
}