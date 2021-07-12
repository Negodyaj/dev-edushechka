using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
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

        public GroupDto AddGroup(GroupDto groupDto)
        {
            return _connection.QuerySingle<GroupDto>
            (
                _groupInsertProcedure,
                new
                {
                    groupDto.Id,
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
            GroupDto result = default;
            _connection.Query<GroupDto, CourseDto, GroupDto>
            (
                _groupSelectByIdProcedure,
                (group, course) =>
                {
                    if (result == null)
                    {
                        result = group;
                        result.Course = course;
                    }
                    else
                    {
                        result.Course = course;
                    }
                    return result;
                },
                new { id },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure
            )
            .FirstOrDefault();
            return result;
        }

        public List<GroupDto> GetGroups()
        {
            return _connection.Query<GroupDto>
            (
                _groupSelectAllProcedure,
                commandType: CommandType.StoredProcedure
            )
            .ToList();
        }

        public GroupDto UpdateGroup(GroupDto groupDto)
        {
            return _connection.QuerySingle<GroupDto>
            (
                _groupUpdateByIdProcedure,
                new
                {
                    groupDto.Id,
                    groupDto.Course,
                    groupDto.GroupStatusId,
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
    }    
}