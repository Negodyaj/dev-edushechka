using System.Data;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private const string _userGroupInsertProcedure = "[dbo].[User_Group_Insert]";
        private const string _userGroupDeleteProcedure = "[dbo].[Tag_Delete]";
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
        public void AddGroupMaterialReference(int materialId, int groupId)
        {
            _connection.Execute(
                "dbo.Group_Material_Insert",
                new
                {
                    materialId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public void RemoveGroupMaterialReference(int materialId, int groupId)
        {
            _connection.Execute(
                "dbo.Group_Material_Delete",
                new
                {
                    materialId,
                    groupId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}