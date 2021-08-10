using Dapper;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
        private const string _groupSelectAllByTaskIdProcedure = "dbo.Group_SelectAllByTaskId";
        private const string _groupSelectGroupsByUserIdProcedure = "dbo.Group_SelectAllByUserId";
        private const string _groupSelectGroupsByLessonIdProcedure = "dbo.Group_SelectAllByLessonId";
        private const string _groupSelectByCourse = "dbo.Group_SelectByCourseId";

        private const string _userGroupInsertProcedure = "dbo.User_Group_Insert";
        private const string _userGroupDeleteProcedure = "dbo.Tag_Delete";
        private const string _insertGroupLesson = "dbo.Group_Lesson_Insert";
        private const string _deleteGroupLesson = "dbo.Group_Lesson_Delete";
        private const string _insertGroupMaterial = "dbo.Group_Material_Insert";
        private const string _deleteGroupMaterial = "dbo.Group_Material_Delete";
        private const string _groupSelectAllByMaterialIdProcedure = "dbo.Group_SelectByMaterialId";

        private const string _groupSelectPresentGroupForStudentByUserId = "dbo.Group_SelectPresentGroupForStudentByUserId";
        public GroupRepository() { }

        public int AddGroup(GroupDto groupDto)
        {
            return _connection.QuerySingle<int>
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

        public void DeleteGroup(int id)
        {
            _connection.Execute
            (
                _groupDeleteProcedure,
                new { Id = id },
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
            return _connection.Execute(
                 _deleteGroupLesson,
                 new
                 {
                     groupId,
                     lessonId
                 },
                 commandType: CommandType.StoredProcedure
             );
        }

        public void AddGroupMaterialReference(int groupId, int materialId)
        {
            _connection.Execute(
                _insertGroupMaterial,
                new
                {
                    groupId,
                    materialId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void RemoveGroupMaterialReference(int groupId, int materialId)
        {
            _connection.Execute(
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

        public List<GroupDto> GetGroupsByMaterialId(int id)
        {
            return _connection.Query<GroupDto>(
                    _groupSelectAllByMaterialIdProcedure,
                    new { id },
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<GroupDto> GetGroupsByUserId(int userId)
        {
            GroupDto result;
            return _connection
                .Query<GroupDto, GroupStatus, GroupDto>(
                    _groupSelectGroupsByUserIdProcedure,
                    (group, groupStatus) =>
                    {
                        result = group;
                        result.GroupStatus = groupStatus;
                        return result;
                    },
                    new { userId },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure
                )
                .ToList();
        }

        public List<GroupDto> GetGroupsByLessonId(int lessonId)
        {
            GroupDto result;
            return _connection
                .Query<GroupDto, GroupStatus, GroupDto>(
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
                )
                .ToList();
        }

        public List<GroupDto> GetGroupsByTaskId(int taskId)
        {
            GroupDto result;
            return _connection
                .Query<GroupDto, GroupStatus, GroupDto>(
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
                )
                .ToList();
        }

        public int GetPresentGroupForStudentByUserId(int userId)
        {
            return _connection.QuerySingle<int>(
                   _groupSelectPresentGroupForStudentByUserId,
                   new { Id = userId },
                   commandType: CommandType.StoredProcedure
              );
        }

        public List<GroupDto> GetGroupsByCourseId(int courseId)
        {
            return _connection.Query<GroupDto>(
                    _groupSelectByCourse,
                    new { courseId },
                    commandType: CommandType.StoredProcedure)
                .ToList();
        }
    }
}