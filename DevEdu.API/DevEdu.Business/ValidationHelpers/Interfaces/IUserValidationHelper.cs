using DevEdu.DAL.Enums;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IUserValidationHelper
    {
        void CheckUserExistence(int userId);
        public void CheckUserBelongToGroup(int groupId, int userId, Role role);
        public void CheckUserBelongToGroup(int groupId, int userId, List<Role> roles);
        public void CheckAuthorizationUserToGroup(int groupId, int userId, Role role);
    }
}