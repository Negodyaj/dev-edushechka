using DevEdu.DAL.Enums;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        void CheckUserExistence(int userId);
        void CheckAuthorizationUserToGroup(int groupId, int userId, Role role);
    }
}