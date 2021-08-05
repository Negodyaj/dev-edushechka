using DevEdu.DAL.Enums;

namespace DevEdu.Business
{
    public static class UserIdentityInfoExtensions
    {
        public static bool IsAdmin(this UserIdentityInfo userInfo)
        {
            return userInfo.Roles.Contains(Role.Admin);
        }

        public static bool IsManager(this UserIdentityInfo userInfo)
        {
             return userInfo.Roles.Contains(Role.Manager);
        }

        public static bool IsMethodist(this UserIdentityInfo userInfo)
        {
            return userInfo.Roles.Contains(Role.Methodist);
        }

        public static bool IsTeacher(this UserIdentityInfo userInfo)
        {
            return userInfo.Roles.Contains( Role.Teacher);
        }

        public static bool IsTutor(this UserIdentityInfo userInfo)
        {
            return userInfo.Roles.Contains(Role.Tutor);
        }

        public static bool IsStudent(this UserIdentityInfo userInfo)
        {
            return userInfo.Roles.Contains(Role.Student);
        }
    }
}