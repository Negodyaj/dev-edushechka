using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITaskValidationHelper
    {
        TaskDto GetTaskByIdAndThrowIfNotFound(int taskId);
        void CheckUserAccessToTask(int taskId, int userId);
        void CheckMethodistAccessToTask(TaskDto taskDto, int userId);
        TaskDto GetTaskAllowedToUser(int taskId, int userId);
    }
}