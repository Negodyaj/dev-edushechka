using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITaskValidationHelper
    {
        public TaskDto GetTaskByIdAndThrowIfNotFound(int taskId);
        public void CheckUserAccessToTask(int taskId, int userId);
    }
}