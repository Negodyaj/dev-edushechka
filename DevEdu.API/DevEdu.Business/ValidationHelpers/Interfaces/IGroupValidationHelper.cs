using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        Task CheckGroupExistence(int groupId);
        void CheckAccessGetGroupMembers(int groupId, UserIdentityInfo userInfo);
        void TmpAccess(UserIdentityInfo userInfo , int id2, int id3 = 0);
    }
}