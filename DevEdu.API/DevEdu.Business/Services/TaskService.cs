using System.Collections.Generic;
using System.Linq;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;
        private readonly ITaskValidationHelper _taskValidationHelper;

        public TaskService(
            ITaskRepository taskRepository,
            ICourseRepository courseRepository,
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository, 
            ITaskValidationHelper taskValidationHelper
        )
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _taskValidationHelper = taskValidationHelper;
        }

        public TaskDto GetTaskById(int id)
        {
            // check if task exists
            _taskValidationHelper.CheckTaskExistence(id);
            var taskDto = _taskRepository.GetTaskById(id);
            return taskDto;
        }

        public TaskDto GetTaskByIdWithValidation(int id, int userId)
        {
            _taskValidationHelper.CheckTaskExistenceWithValidation(id, userId);

            // check if task exists
            var taskDto = GetTaskById(id);
            return taskDto;
        }

        public TaskDto GetTaskWithCoursesById(int id, int userId)
        {
            var taskDto = GetTaskByIdWithValidation(id, userId);
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(id);
            return taskDto;
        }

        public TaskDto GetTaskWithAnswersById(int id, int userId)
        {
            var taskDto = GetTaskByIdWithValidation(id, userId);
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetStudentAnswersToTaskByTaskId(id);
            return taskDto;
        }

        public List<TaskDto> GetTasks()
        {
            var tasks = _taskRepository.GetTasks();
            if (tasks == default)
                throw new EntityNotFoundException(ServiceMessages.NotFounAnyTaskMessage);
            return tasks;
        }

        public int AddTask(TaskDto taskDto)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            if (taskDto.Tags == null || taskDto.Tags.Count == 0)
                return taskId;

            taskDto.Tags.ForEach(tag => AddTagToTask(taskId, tag.Id));
            return taskId;
        }

        public TaskDto UpdateTask(TaskDto taskDto)
        {
            _taskValidationHelper.CheckTaskExistence(taskDto.Id);
            _taskRepository.UpdateTask(taskDto);
            return _taskRepository.GetTaskById(taskDto.Id);
        }

        public void DeleteTask(int id)
        {
            _taskValidationHelper.CheckTaskExistence(id);
            _taskRepository.DeleteTask(id);
        }

        public int AddTagToTask(int taskId, int tagId) => _taskRepository.AddTagToTask(taskId, tagId);

        public void DeleteTagFromTask(int taskId, int tagId) => _taskRepository.DeleteTagFromTask(taskId, tagId);
        public List<GroupTaskDto> GetGroupsByTaskId(int taskId) => _taskRepository.GetGroupsByTaskId(taskId);
    }
}