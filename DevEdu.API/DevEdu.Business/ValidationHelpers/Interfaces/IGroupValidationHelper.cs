namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        void CheckGroupExistence(int groupId);
        void CheckAuthorizationException(int userId);
    }
}