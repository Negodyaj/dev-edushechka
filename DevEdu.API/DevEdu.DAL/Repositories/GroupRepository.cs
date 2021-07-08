using System.Data;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private const string _userGroupInsertProcedure = "dbo.User_Group_Insert";
        private const string _userGroupDeleteProcedure = "dbo.Tag_Delete";
        private const string _insertGroupLesson = "dbo.Group_Lesson_Insert";
        private const string _deleteGroupLesson = "dbo.Group_Lesson_Delete";
        private const string _groupMaterialInsertProcedure = "dbo.Group_Material_Insert";
        private const string _groupMaterialDeleteProcedure = "dbo.Group_Material_Delete";

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

        public int AddGroupMaterialReference(int materialId, int groupId)
        {
            return _connection.Execute(
                _groupMaterialInsertProcedure,
                new
                {
                    materialId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public int RemoveGroupMaterialReference(int materialId, int groupId)
        {
            return _connection.Execute(
                _groupMaterialDeleteProcedure,
                new
                {
                    materialId,
                    groupId
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