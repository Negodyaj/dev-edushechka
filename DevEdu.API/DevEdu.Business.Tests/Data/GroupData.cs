using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Tests
{
    public class GroupData
    {
        public const int ExpectedAffectedRows = 1;
        public const int GroupId = 1;
        public const int MaterialId = 1;

        public static GroupDto GetGroupDto()
        {
            return new GroupDto();
        }

        public static List<GroupDto> GetGroupsDto()
        {
            return new List<GroupDto>();
        }
    }
}