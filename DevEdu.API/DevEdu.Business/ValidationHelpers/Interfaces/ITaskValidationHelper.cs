using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITaskValidationHelper
    {
        public TaskDto GetTaskByIdAndThrowIfNotFound(int taskId);
        public void CheckUserAccessToTask(int taskId, int userId);
        public TaskDto GetTaskAllowedToUser(int taskId, int userId);
        public List<TaskDto> GetTasksAllowedToUser(List<TaskDto> tasks, int userId);
    }
}