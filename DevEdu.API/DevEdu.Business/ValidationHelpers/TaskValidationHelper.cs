using System.Collections.Generic;
using System.Linq;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class TaskValidationHelper : ITaskValidationHelper
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IGroupRepository _groupRepository;

        public TaskValidationHelper(ITaskRepository taskRepository, IGroupRepository groupRepository)
        {
            _taskRepository = taskRepository;
            _groupRepository = groupRepository;
        }

        public TaskDto GetTaskByIdAndThrowIfNotFound(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), taskId));
            return task;
        }

        public void CheckUserAccessToTask(int taskId, int userId)
        {
            var groupsByTask = _groupRepository.GetGroupsByTaskId(taskId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                throw new AuthorizationException(string.Format(ServiceMessages.EntityDoesntHaveAcessMessage, "user", userId, "task", taskId));
        }

        public TaskDto GetTaskAllowedToUser(int taskId, int userId)
        {
            var groupsByTask = _groupRepository.GetGroupsByTaskId(taskId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByTask.FirstOrDefault(gt => groupsByUser.Any(gu => gu.Id == gt.Id));
            if (result == default)
                return null;
            return _taskRepository.GetTaskById(taskId);
        }

        public List<TaskDto> GetTasksAllowedToUser(List<TaskDto> tasks, int userId)
        {
            var taskDtos = new List<TaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(GetTaskAllowedToUser(task.Id, userId));
            }
            return taskDtos;
        }
    }
}