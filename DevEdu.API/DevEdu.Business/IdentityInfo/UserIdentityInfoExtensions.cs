using System.Linq;
using DevEdu.DAL.Enums;

namespace DevEdu.Business
{
    public static class UserIdentityInfoExtensions
    {
        public static bool IsAdmin(this UserIdentityInfo userInfo)
        {
            var admin = userInfo.Roles.FirstOrDefault(r => r == Role.Admin);
            return admin != default;
        }

        public static bool IsManager(this UserIdentityInfo userInfo)
        {
            var manager = userInfo.Roles.FirstOrDefault(r => r == Role.Manager);
            return manager != default;
        }

        public static bool IsMethodist(this UserIdentityInfo userInfo)
        {
            var methodist = userInfo.Roles.FirstOrDefault(r => r == Role.Methodist);
            return methodist != default;
        }

        public static bool IsTeacher(this UserIdentityInfo userInfo)
        {
            var teacher = userInfo.Roles.FirstOrDefault(r => r == Role.Teacher);
            return teacher != default;
        }

        public static bool IsTutor(this UserIdentityInfo userInfo)
        {
            var tutor = userInfo.Roles.FirstOrDefault(r => r == Role.Tutor);
            return tutor != default;
        }

        public static bool IsStudent(this UserIdentityInfo userInfo)
        {
            var student = userInfo.Roles.FirstOrDefault(r => r == Role.Student);
            return student != default;
        }
    }
}