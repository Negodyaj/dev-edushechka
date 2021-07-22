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


        private const string _userGroupInsertProcedure = "dbo.User_Group_Insert";
        private const string _userGroupDeleteProcedure = "dbo.Tag_Delete";
        private const string _insertGroupLesson = "dbo.Group_Lesson_Insert";
        private const string _deleteGroupLesson = "dbo.Group_Lesson_Delete";
        private const string _insertGroupMaterial = "dbo.Group_Material_Insert";
        private const string _deleteGroupMaterial = "dbo.Group_Material_Delete";
        private const string _groupSelectAllByMaterialIdProcedure = "dbo.Group_SelectByMaterialId";

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
            var userDictionary = new Dictionary<int, UserDto>();
            GroupDto result = default;
            UserDto resultUser = default;
            int index = 0;
            return _connection
                .Query<GroupDto, CourseDto, Role, UserDto, GroupDto>
            (
                _groupSelectByIdProcedure,
                (group, course, role, user) =>
                {
                    if (!userDictionary.TryGetValue(user.Id, out resultUser))
                    {
                        result = group;
                        result.Course = course;
                        result.Users = new List<UserDto> { user };
                        result.Users[index].Roles = new List<Role> { role };
                        userDictionary.Add(user.Id, resultUser);
                    }
                    else
                    {
                        result.Users.Add(user);
                        result.Users[index].Roles.Add(role);
                    }
                    index++;
                    return result;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            )
            .FirstOrDefault();
        }

        public List<GroupDto> GetGroups()
        {
            var groupDictionary = new Dictionary<int, GroupDto>();
            GroupDto result = default;
            return _connection
                .Query<GroupDto, CourseDto, GroupDto>
            (
                _groupSelectAllProcedure,
                (group, course) =>
                {
                    if (!groupDictionary.TryGetValue(group.Id, out result))
                    {
                        result = group;
                        result.Course = course;
                    }
                    groupDictionary.Add(group.Id, result);              
                    return result;
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            )
            .Distinct()
            .ToList();
        }

        public void UpdateGroup(int id, GroupDto groupDto)
        {
            _connection.Execute
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

        public void AddGroupLesson(int groupId, int lessonId)
        {
            _connection.Execute(
                _insertGroupLesson,
                new
                {
                    groupId,
                    lessonId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void RemoveGroupLesson(int groupId, int lessonId)
        {
            _connection.Execute(
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