using DevEdu.DAL.Models;
using Dapper;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private const string _userAddProcedure = "dbo.User_Insert";
        private const string _userSelectByIdProcedure = "dbo.User_SelectById";
        private const string _userSelectAllProcedure = "dbo.User_SelectAll";
        private const string _userUpdateProcedure = "dbo.User_Update";
        private const string _userDeleteProcedure = "dbo.User_Delete";

        private const string _userRoleAddProcedure = "dbo.User_Role_Insert";
        private const string _userRoleDeleteProcedure = "dbo.User_Role_Delete";

        public UserRepository() { }

        public int AddUser(UserDto user)
        {
            return _connection.QuerySingle<int>(
               _userAddProcedure,
                new
                {
                    user.FisrtName,
                    user.LastName,
                    user.Patronymic,
                    user.Email,
                    user.Username,
                    user.Password,
                    user.ContractNumber,
                    user.CityId,
                    user.BirthDate,
                    user.GitHubAccount,
                    user.Photo,
                    user.PhoneNumer
                },
            commandType: CommandType.StoredProcedure);
        }

        public UserDto SelectUserById(int id)
        {
            return _connection.QuerySingleOrDefault<UserDto>(
                _userSelectByIdProcedure,
                new { id },
            commandType: CommandType.StoredProcedure);
        }

        public List<UserDto> SelectUsers()
        {
            return _connection
                .Query<UserDto>(
                _userSelectAllProcedure,
            commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public void UpdateUser(UserDto user)
        {
            _connection.Execute(
                _userUpdateProcedure,
                new
                {
                    user.Id,
                    user.FisrtName,
                    user.LastName,
                    user.Patronymic,
                    user.Username,
                    user.CityId,
                    user.GitHubAccount,
                    user.Photo,
                    user.PhoneNumer
                },
            commandType: CommandType.StoredProcedure
            );
        }

        public void DeleteUser(int id)
        {
            _connection.Execute(
                _userDeleteProcedure,
                new { id },
            commandType: CommandType.StoredProcedure
            );
        }

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