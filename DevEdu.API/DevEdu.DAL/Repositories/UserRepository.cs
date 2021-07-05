using DevEdu.DAL.Models;
using Dapper;
using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository()
        {
            _insertProcedure = "dbo.User_Insert";
            _selectByIdProcedure = "dbo.User_SelectById";
            _selectAllProcedure = "dbo.User_SelectAll";
            _updateProcedure = "dbo.User_Update";
            _deleteProcedure = "dbo.User_Delete";
        }

        public int AddUser(UserDto user)
        {
            return _connection.QuerySingle<int>(
               _insertProcedure,
                new
                {
                    user.Name,
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
                _selectByIdProcedure,
                id,
            commandType: CommandType.StoredProcedure);
        }

        public List<UserDto> SelectUsers()
        {
            return _connection
                .Query<UserDto>(
                _selectAllProcedure,
            commandType: CommandType.StoredProcedure)
                .ToList();
        }

        public void UpdateUser(UserDto user)
        {
            _connection.Execute(
                _updateProcedure,
                new
                {
                    user.Id,
                    user.Name,
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
                _deleteProcedure,
                new
                {
                    id,
                },
            commandType: CommandType.StoredProcedure
            );
        }
    }
}
