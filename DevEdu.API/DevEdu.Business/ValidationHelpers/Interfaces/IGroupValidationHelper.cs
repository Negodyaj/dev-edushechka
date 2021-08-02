using DevEdu.DAL.Enums;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IGroupValidationHelper
    {
        void CheckGroupExistence(int groupId);
    }
}