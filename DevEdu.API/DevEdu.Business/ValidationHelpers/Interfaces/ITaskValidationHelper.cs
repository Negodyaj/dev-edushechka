using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface ITaskValidationHelper
    {
        public TaskDto GetTaskByIdAndThrowIfNotFound(int taskId);
        public AuthorizationException CheckUserAccessToTask(int taskId, int userId);
        public AuthorizationException CheckMethodistAccessToTask(TaskDto taskDto, int userId);
        public TaskDto GetTaskAllowedToUser(int taskId, int userId);
        public List<TaskDto> GetTasksAllowedToMethodist(List<TaskDto> taskDtos);
    }
}