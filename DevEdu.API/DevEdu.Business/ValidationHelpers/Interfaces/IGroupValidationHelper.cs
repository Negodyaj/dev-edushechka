using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        Task<GroupDto> CheckGroupExistence(int groupId);
        void CheckUserInGroupExistence(int groupId, int userId);
        void CheckAccessGetGroupMembers(int groupId, UserIdentityInfo userInfo);
        void CheckAccessGroup(UserIdentityInfo userInfo , int groupId);
        void CheckAccessGroupAndMaterial(UserIdentityInfo userInfo , int groupId, int materialId);
        void CheckAccessGroupAndLesson(UserIdentityInfo userInfo , int groupId, int lessonId);
        void CheckAccessGroupAndUser(UserIdentityInfo userInfo , int groupId, int userId);
        void CheckAccessGroupAndTask(UserIdentityInfo userInfo , int groupId, int taskId);
    }
}