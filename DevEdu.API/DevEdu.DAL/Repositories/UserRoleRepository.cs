using System.Data;
using Dapper;
using DevEdu.DAL.Models;


namespace DevEdu.DAL.Repositories
{
    public class UserRoleRepository : BaseRepository, IUserRoleRepository
    {
        public UserRoleRepository()
        {
            _insertProcedure = "dbo.User_Role_Insert";
            _deleteProcedure = "dbo.User_Role_Delete";
        }

        public int AddUserRole(int userId, int roleId)
        {
            return _connection.QuerySingle<int>(
                _insertProcedure,
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
                _deleteProcedure,
                new
                {
                    userId,
                    roleId
                },
                commandType: CommandType.StoredProcedure);
        }
    }
}
