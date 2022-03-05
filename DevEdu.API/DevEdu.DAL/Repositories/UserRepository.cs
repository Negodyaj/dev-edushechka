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
        private const string _userUpdatePasswordProcedure = "dbo.User_UpdatePassword";
        private const string _userDeleteProcedure = "dbo.User_Delete";
        private const string _userRoleInsertProcedure = "dbo.User_Role_Insert";
        private const string _userRoleDeleteProcedure = "dbo.User_Role_Delete";

        public UserRepository(IOptions<DatabaseSettings> options) : base(options) { }

        public async Task<int> AddUserAsync(UserDto user)
        {
            return await _connection.QuerySingleAsync<int>(
                _userInsertProcedure,
                new
                {
                    user.FirstName,
                    user.LastName,
                    user.Patronymic,
                    user.Email,
                    user.Username,
                    user.Password,
                    CityId = (int)user.City,
                    user.BirthDate,
                    user.GitHubAccount,
                    user.Photo,
                    user.PhoneNumber
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task<UserDto> GetUserByIdAsync(int id)
        {
            UserDto result = default;

            return (await _connection
                .QueryAsync<UserDto, City, Role, UserDto>(
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
                commandType: CommandType.StoredProcedure))
                .FirstOrDefault();
        }

        public async Task<UserDto> GetUserByEmailAsync(string email)
        {
            UserDto result = default;

            return (await _connection
                .QueryAsync<UserDto, Role, UserDto>(
                    _userSelectByEmailProcedure,
                    (user, role) =>
                    {
                        if (result == null)
                        {
                            result = user;
                            result.Roles = new List<Role> { role };
                        }
                        else
                        {
                            result.Roles.Add(role);
                        }
                        return result;
                    },
                    new { email },
                    splitOn: "Id",
                    commandType: CommandType.StoredProcedure))
                .FirstOrDefault();
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var userDictionary = new Dictionary<int, UserDto>();

            return (await _connection
                .QueryAsync<UserDto, City, Role, UserDto>(
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
                commandType: CommandType.StoredProcedure))
                .Distinct()
                .ToList();
        }

        public async Task UpdateUserAsync(UserDto user)
        {
            await _connection.ExecuteAsync(
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

        public async Task UpdateUserPasswordAsync(UserDto user)
        {
            await _connection.ExecuteAsync(
                 _userUpdatePasswordProcedure,
                 new
                 {
                     user.Id,
                     user.Password
                 },
                 commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteUserAsync(int id)
        {
            await _connection.ExecuteAsync(
                 _userDeleteProcedure,
                 new { id },
                 commandType: CommandType.StoredProcedure);
        }

        public async Task AddUserRoleAsync(int userId, int roleId)
        {
           await _connection.QueryFirstOrDefaultAsync<int>(
                _userRoleInsertProcedure,
                new
                {
                    userId,
                    roleId
                },
                commandType: CommandType.StoredProcedure);
        }

        public async Task DeleteUserRoleAsync(int userId, int roleId)
        {
           await _connection.ExecuteAsync(
                _userRoleDeleteProcedure,
                new
                {
                    userId,
                    roleId
                },
                commandType: CommandType.StoredProcedure);
        }

        //public async Task<List<UserDto>> GetUsersByGroupIdAndRoleAsync(int groupId, int role)
        //{

        //    return (await _connection.QueryAsync<UserDto>
        //    (
        //        _userSelectByGroupIdAndRole,
        //        new
        //        {
        //            groupId,
        //            roleId = role
        //        },
        //        commandType: CommandType.StoredProcedure
        //    )).ToList();
        //}

        public async Task<List<UserDto>> GetUsersByGroupIdAndRoleAsync(int groupId, int role)
        {
            return (await _connection.QueryAsync<UserDto>
            (
                _userSelectByGroupIdAndRole,
                new
                {
                    groupId,
                    roleId = role
                },
                commandType: CommandType.StoredProcedure
            )).ToList();
        }
    }
}