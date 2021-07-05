using Dapper;
using System.Data;

namespace DevEdu.DAL.Repositories
{
    public class User_GroupRepository : BaseRepository, IUser_GroupRepository
    {
        private const string User_GroupInsertProcedure = "[dbo].[User_Group_Insert]";
        private const string User_GroupDeleteProcedure = "[dbo].[Tag_Delete]";
        public void AddTag(int groupId, int userId, int roleId)
        {
            _connection.Execute(User_GroupInsertProcedure, new { groupId, userId, roleId }, commandType: CommandType.StoredProcedure);
        }
        public void DeleteTag(int userId, int groupId)
        {
            _connection.Execute(User_GroupDeleteProcedure, new { userId, groupId }, commandType: CommandType.StoredProcedure);
        }
    }
}
