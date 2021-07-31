using System.Collections.Generic;
using System.Linq;
using DevEdu.DAL.Enums;

namespace DevEdu.Business
{
    public static class CheckerRole
    {
        public static bool Admin(List<Role> roles)
        {
            var admin = roles.FirstOrDefault(r => r == Role.Admin);
            return admin != default;
        }

        public static bool Manager(List<Role> roles)
        {
            var admin = roles.FirstOrDefault(r => r == Role.Manager);
            return admin != default;
        }

        public static bool Methodist(List<Role> roles)
        {
            var admin = roles.FirstOrDefault(r => r == Role.Methodist);
            return admin != default;
        }
        public static bool Teacher(List<Role> roles)
        {
            var admin = roles.FirstOrDefault(r => r == Role.Teacher);
            return admin != default;
        }

        public static bool Tutor(List<Role> roles)
        {
            var admin = roles.FirstOrDefault(r => r == Role.Tutor);
            return admin != default;
        }

        public static bool Student(List<Role> roles)
        {
            var admin = roles.FirstOrDefault(r => r == Role.Student);
            return admin != default;
        }
    }
}