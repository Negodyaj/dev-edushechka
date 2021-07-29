namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        void CheckGroupExistence(int groupId);
        void CheckAccessGetGroupMembers(int groupId, int userId);
    }
}