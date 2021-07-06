using System.Data;
using Dapper;

namespace DevEdu.DAL.Repositories
{
    public class GroupRepository : BaseRepository, IGroupRepository
    {
        private const string _user_GroupInsertProcedure = "[dbo].[User_Group_Insert]";
        private const string _user_GroupDeleteProcedure = "[dbo].[Tag_Delete]";
        public void AddUserToGroup(int groupId, int userId, int roleId)
        {
            _connection.Execute(_user_GroupInsertProcedure,
                new 
                { 
                    groupId, 
                    userId, 
                    roleId 
                }, 
                commandType: CommandType.StoredProcedure);
        }
        public void DeleteUserFromGroup(int userId, int groupId)
        {
            _connection.Execute(_user_GroupDeleteProcedure, 
                new 
                { 
                    userId, 
                    groupId 
                }, 
                commandType: CommandType.StoredProcedure);
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