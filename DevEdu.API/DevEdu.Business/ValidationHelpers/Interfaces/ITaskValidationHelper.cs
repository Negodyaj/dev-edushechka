using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITaskValidationHelper
    {
        public void CheckTaskExistence(int taskId);
        public void CheckTaskExistenceWithValidation(int taskId, int userId);
    }
}