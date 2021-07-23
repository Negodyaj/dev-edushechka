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
        private readonly IUserRepository _userRepository;

        public TaskValidationHelper(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public void CheckTaskExistence(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(task), taskId));
           //return task;
        }

        public void CheckTaskExistenceWithValidation(int taskId, int userId)
        {
            var groupsByTask = _taskRepository.GetGroupsByTaskId(taskId);
            var groupsByUser = _userRepository.GetGroupsByUserId(userId);
            List<GroupDto> grByT = new List<GroupDto>();
            List<GroupDto> grByU = new List<GroupDto>();
            foreach (var group in groupsByTask)
                grByT.Add(group.Group);
            foreach (var group in groupsByUser)
                grByU.Add(group.Group);

            var result = grByT.FirstOrDefault(gt => grByU.Any(gu => gu.Id == gt.Id));
            if (result == default)
                throw new AuthorizationException($"user with id = {userId} doesn't relate to task with id = {taskId}");
        }
    }
}