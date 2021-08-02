using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class UserTokenData
    {
        public static UserIdentityInfo GetUserIdentityWithRole(Enum role)
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    (Role)role
                }
            };
        }

        public static UserIdentityInfo GetUserTokenWithAdminRole()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Admin
                }
            };
        }

        public static UserIdentityInfo GetUserTokenWithManagerRole()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Manager
                }
            };
        }

        public static UserIdentityInfo GetUserTokenWithMethodistRole()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Methodist
                }
            };
        }

        public static UserIdentityInfo GetUserTokenWithTeacherRole()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Teacher
                }
            };
        }

        public static UserIdentityInfo GetUserTokenWithTutorRole()
        {
            return new UserIdentityInfo
            {
                UserId = 1,
                Roles = new List<Role>
                {
                    Role.Tutor
                }
            };
        }

        public static UserIdentityInfo GetUserTokenWithStudentRole()
        {
            return new UserIdentityInfo
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