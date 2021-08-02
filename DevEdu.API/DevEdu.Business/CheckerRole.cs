using System.Collections.Generic;
using System.Linq;
using DevEdu.DAL.Enums;

namespace DevEdu.Business
{
    public static class CheckerRole
    {
        public static bool IsAdmin(List<Role> roles)
        {
            var admin = roles.FirstOrDefault(r => r == Role.Admin);
            return admin != default;
        }

        public static bool IsManager(List<Role> roles)
        {
            var manager = roles.FirstOrDefault(r => r == Role.Manager);
            return manager != default;
        }

        public static bool IsMethodist(List<Role> roles)
        {
            var methodist = roles.FirstOrDefault(r => r == Role.Methodist);
            return methodist != default;
        }

        public static bool IsTeacher(List<Role> roles)
        {
            var teacher = roles.FirstOrDefault(r => r == Role.Teacher);
            return teacher != default;
        }

        public static bool IsTutor(List<Role> roles)
        {
            var tutor = roles.FirstOrDefault(r => r == Role.Tutor);
            return tutor != default;
        }

        public static bool IsStudent(List<Role> roles)
        {
            var student = roles.FirstOrDefault(r => r == Role.Student);
            return student != default;
        }
    }
}