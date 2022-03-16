using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        bool CheckAccessGetGroupMembers(int groupId, UserIdentityInfo userInfo);
        Task<GroupDto> CheckGroupExistenceAsync(int groupId);
        Task CheckUserInGroupExistenceAsync(int groupId, int userId);
        Task CompareStartEndDateAsync(DateTime startDate, DateTime endDate);
    }
}