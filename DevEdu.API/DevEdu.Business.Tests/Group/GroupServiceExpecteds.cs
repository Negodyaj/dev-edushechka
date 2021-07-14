using DevEdu.DAL.Models;
using System.Collections;

namespace DevEdu.Business.Tests.Group
{
    public class GroupServiceExpecteds
    {
        public static IEnumerable AddGroup()
        {
            yield return new object[]
            {
                new GroupDto{Id = 1 },
                new GroupDto{Id = 1 },
                new GroupDto{Id = 1 }
            };
            yield return new object[]
            {
                new GroupDto{Id = 2 },
                new GroupDto{Id = 2 },
                new GroupDto{Id = 2 }
            };
        }
        
        public static IEnumerable DeleteGroup()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }
        
        public static IEnumerable GetGroup()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable GetGroups()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable AddGroupLesson()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable RemoveGroupLesson()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable UpdateGroup()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable AddGroupMaterialReference()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable RemoveGroupMaterialReference()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable AddUserToGroup()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }

        public static IEnumerable DeleteUserFromGroup()
        {
            yield return new object[]
            {
            };
            yield return new object[]
            {
            };
        }
    }
}