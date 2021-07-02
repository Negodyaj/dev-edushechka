using DevEdu.DAL.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public class UserRepository
    {
        string connectionString = "Data Source=80.78.240.16;Initial Catalog = DevEdu; Persist Security Info=True;User ID = student;Password=qwe!23; Pooling=False;MultipleActiveResultSets=False;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False";

        private readonly IDbConnection _connection;

        public UserRepository()
        {
            _connection = new SqlConnection(connectionString);
        }

        public int AddUser(UserDto user)
        {
            return _connection.QueryFirst<int>("dbo.User_Insert", new
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
            return _connection.QueryFirst<UserDto>("dbo.User_SelectById",
                id,
            commandType: CommandType.StoredProcedure);
        }

        public List<UserDto> SelectUsers()
        {
            return _connection.Query<UserDto>("dbo.User_SelectAll",
            commandType: CommandType.StoredProcedure).
            AsList<UserDto>();
        }

        public int UpdateUser(int id, UserDto user)
        {
            return _connection.QueryFirst<int>("dbo.User_Update", new
            {
                id,
                user.Name,
                user.Username,
                user.CityId,
                user.GitHubAccount,
                user.Photo,
                user.PhoneNumer

            },
            commandType: CommandType.StoredProcedure);
        }

        public int DeleteUser(int id)
        {
            return _connection.QueryFirst<int>("dbo.User_Delete", new
            {
                id,
            },
            commandType: CommandType.StoredProcedure);
        }
    }
}
