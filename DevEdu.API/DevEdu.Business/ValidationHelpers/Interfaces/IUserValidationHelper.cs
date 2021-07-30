using DevEdu.DAL.Enums;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        void CheckUserExistence(int userId);
        public void CheckUserBelongToGroup(int groupId, int userId, Role role);
        public void CheckAuthorizationUserToGroup(int groupId, int userId, Role role);
    }
}