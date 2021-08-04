using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;
using System;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class UserIdentityInfoData
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

        public static UserIdentityInfo GetUserIdentityWithAdminRole()
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

        public static UserIdentityInfo GetUserIdentityWithManagerRole()
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

        public static UserIdentityInfo GetUserIdentityWithMethodistRole()
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

        public static UserIdentityInfo GetUserIdentityWithTeacherRole()
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

        public static UserIdentityInfo GetUserIdentityWithTutorRole()
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

        public static UserIdentityInfo GetUserIdentityWithStudentRole()
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