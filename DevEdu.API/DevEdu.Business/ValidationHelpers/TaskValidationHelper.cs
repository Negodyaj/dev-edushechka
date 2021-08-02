using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class TaskValidationHelper : ITaskValidationHelper
    {
        private readonly ITaskRepository _taskRepository;

        public TaskValidationHelper(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public void CheckTaskExistence(int taskId)
        {
            var task = _taskRepository.GetTaskById(taskId);
            if (task == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(task), taskId));
        }
    }
}