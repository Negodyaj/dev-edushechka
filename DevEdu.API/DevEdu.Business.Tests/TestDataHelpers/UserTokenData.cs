using System.Collections.Generic;
using DevEdu.DAL.Enums;

namespace DevEdu.Business.Tests
{
    public class UserTokenData
    {
        public static UserToken GetUserTokenWithCustomRole(int role)
        {
            return new UserToken
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    (Role)role
                }
            };
        }

        public static UserToken GetUserTokenWithAdminRole()
        {
            return new UserToken
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Admin
                }
            };
        }

        public static UserToken GetUserTokenWithManagerRole()
        {
            return new UserToken
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Manager
                }
            };
        }

        public static UserToken GetUserTokenWithMethodistRole()
        {
            return new UserToken
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Methodist
                }
            };
        }

        public static UserToken GetUserTokenWithTeacherRole()
        {
            return new UserToken
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Teacher
                }
            };
        }

        public static UserToken GetUserTokenWithTutorRole()
        {
            return new UserToken
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Tutor
                }
            };
        }

        public static UserToken GetUserTokenWithStudentRole()
        {
            return new UserToken
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Student
                }
            };
        }
    }
}