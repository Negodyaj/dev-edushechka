using DevEdu.DAL.Enums;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        void CheckGroupExistence(int groupId);
        public void CheckUserBelongToGroup(int groupId, int userId, Role role);
        public void CheckTeacherBelongToGroup(int groupId, int teacherId, Role role);
    }
}