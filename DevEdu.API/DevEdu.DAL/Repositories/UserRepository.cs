using Dapper;
using DevEdu.Core;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private const string _userInsertProcedure = "dbo.User_Insert";
        private const string _userSelectByIdProcedure = "dbo.User_SelectById";
        private const string _userSelectByEmailProcedure = "dbo.User_SelectByEmail";
        private const string _userSelectAllProcedure = "dbo.User_SelectAll";
        private const string _userSelectByGroupIdAndRole = "dbo.User_SelectByGroupIdAndRole";
        private const string _userUpdateProcedure = "dbo.User_Update";
        private const string _userDeleteProcedure = "dbo.User_Delete";
        private const string _userRoleInsertProcedure = "dbo.User_Role_Insert";
        private const string _userRoleDeleteProcedure = "dbo.User_Role_Delete";

        public UserRepository(IOptions<DatabaseSettings> options) : base(options) { }

        public int AddUser(UserDto user)
        {
            return _connection.QuerySingle<int>(
                _userInsertProcedure,
                new
                {
                    user.FirstName,
                    user.LastName,
                    user.Patronymic,
                    user.Email,
                    user.Username,
                    user.Password,
                    user.ContractNumber,
                    CityId = (int)user.City,
                    user.BirthDate,
                    user.GitHubAccount,
                    user.Photo,
                    user.PhoneNumber
                },
                commandType: CommandType.StoredProcedure);
        }

        public UserDto GetUserById(int id)
        {
            UserDto result = default;
            return _connection
                .Query<UserDto, City, Role, UserDto>(
                _userSelectByIdProcedure,
                (user, city, role) =>
                {
                    if (result == null)
                    {
                        result = user;
                        result.City = city;
                        result.Roles = new List<Role> { role };
                    }
                    else
                    {
                        result.Roles.Add(role);
                    }
                    return result;
                },
                new { id },
                splitOn: "id",
                commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public UserDto GetUserByEmail(string email)
        {
            UserDto result = default;
            return _connection
                .Query<UserDto, City, Role, UserDto>(
                    _userSelectByEmailProcedure,
                    (user, city, role) =>
                    {
                        if (result == null)
                        {
                            result = user;
                            result.City = city;
                            result.Roles = new List<Role> { role };
                        }
                        else
                        {
                            result.Roles.Add(role);
                        }
                        return result;
                    },
                    new { email },
                    splitOn: "id",
                    commandType: CommandType.StoredProcedure)
                .FirstOrDefault();
        }

        public List<UserDto> GetAllUsers()
        {
            var userDictionary = new Dictionary<int, UserDto>();
            return _connection
                .Query<UserDto, City, Role, UserDto>(
                _userSelectAllProcedure,
                (user, city, role) =>
                {
                    if (!userDictionary.TryGetValue(user.Id, out UserDto userEntry))
                    {
                        userEntry = user;
                        userEntry.City = city;
                        userEntry.Roles = new List<Role>();
                        userDictionary.Add(user.Id, userEntry);
                    }
                    userEntry.Roles.Add(role);

                    return userEntry;
                },
                splitOn: "Id",
                commandType: CommandType.StoredProcedure)
                .Distinct()
                .ToList();
        }

        public void UpdateUser(UserDto user)
        {
            _connection.Execute(
                _userUpdateProcedure,
                new
                {
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Patronymic,
                    user.Username,
                    CityId = (int)user.City,
                    user.GitHubAccount,
                    user.Photo,
                    user.PhoneNumber
                },
                commandType: CommandType.StoredProcedure);
        }

        public void DeleteUser(int id)
        {
            _connection.Execute(
                _userDeleteProcedure,
                new { id },
                commandType: CommandType.StoredProcedure);
        }

        public void AddUserRole(int userId, int roleId)
        {
            _connection.QuerySingleOrDefault<int>(
                _userRoleInsertProcedure,
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

        public List<UserDto> GetUsersByGroupIdAndRole(int groupId, int roleId)
        {
            var www = _connection.Query<UserDto>
            (
                _userSelectByGroupIdAndRole,
                new
                {
                    groupId,
                    roleId
                },
                commandType: CommandType.StoredProcedure
            ).ToList();
            return www;
        }

        public async Task<List<UserDto>> GetUsersByGroupIdAndRoleAsync(int groupId, int roleId)
        {
            return (List<UserDto>)await _connection.QueryAsync<UserDto>
            (
                _userSelectByGroupIdAndRole,
                new
                {
                    groupId,
                    roleId
                },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}