using System.Data;
using Dapper;


namespace DevEdu.DAL.Repositories
{
    public class UserRoleRepository : BaseRepository, IUserRoleRepository
    {

        private const string _userRoleAddProcedure = "dbo.User_Role_Insert";
        private const string _userRoleDeleteProcedure = "dbo.User_Role_Delete";

        public UserRoleRepository() { }

        public int AddUserRole(int userId, int roleId)
        {
            return _connection.QuerySingle<int>(
                _userRoleAddProcedure,
                new
                {
                    userId,
                    roleId
                },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteUserRole(int userId, int roleId)
        {
            _connection.Execute(
                _userRoleDeleteProcedure,
                new
                {
                    userId,
                    roleId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
