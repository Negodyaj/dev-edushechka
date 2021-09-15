﻿using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using DevEdu.DAL.Enums;

namespace DevEdu.DAL.Repositories
{
    public interface IUserRepository
    {
        int AddUser(UserDto user);
        void AddUserRole(int userId, Role roleId);
        void DeleteUser(int id);
        void DeleteUserRole(int userId, Role roleId);
        UserDto GetUserById(int id);
        public UserDto GetUserByEmail(string email);
        List<UserDto> GetAllUsers();
        void UpdateUser(UserDto user);
        List<UserDto> GetUsersByGroupIdAndRole(int groupId, int role);
        Task<List<UserDto>> GetUsersByGroupIdAndRoleAsync(int groupId, int role);
    }
}