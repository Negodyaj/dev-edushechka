using DevEdu.DAL.Models;
using Dapper;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using DevEdu.DAL.Enums;

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
                    user.FirstName,
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
                    user.PhoneNumber
                },
            commandType: CommandType.StoredProcedure);
        }

        public UserDto SelectUserById(int id)
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
                        result.CityId = city;
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

        public Dictionary<int, UserDto> SelectUsers()
        {
            var UserDictionary = new Dictionary<int, UserDto>();

            _connection
               .Query<UserDto, City, Role, UserDto>(
               _userSelectAllProcedure,
               (user, city, role) =>
               {
                   UserDto result;

                   if (!UserDictionary.TryGetValue(user.Id, out result))
                   {
                       result = user;
                       result.CityId = city;
                       result.Roles = new List<Role>();
                       UserDictionary.Add(user.Id, result);
                   }
                   else
                   {
                       result.Roles.Add(role);
                   }
                   return result;
               },
               splitOn: "Id",
           commandType: CommandType.StoredProcedure)
               .Distinct()
               .ToList();
            return UserDictionary;
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
                    user.CityId,
                    user.GitHubAccount,
                    user.Photo,
                    user.PhoneNumber
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
            return _connection.QuerySingleOrDefault<int>(
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