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
        private readonly IUserValidationHelper _userValidationHelper;

        public TaskService(
            ITaskRepository taskRepository,
            ICourseRepository courseRepository,
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository, 
            ITaskValidationHelper taskValidationHelper,
            IUserValidationHelper userValidationHelper
        )
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _taskValidationHelper = taskValidationHelper;
            _userValidationHelper = userValidationHelper;
        }

        public TaskDto GetTaskById(int id, int userId)
        {
            _userValidationHelper.CheckUserExistence(userId);
            var taskDto = _taskValidationHelper.CheckTaskExistence(id);
            _taskValidationHelper.CheckTaskExistenceWithValidation(id, userId);
            // check if task exists
            return taskDto;
        }

        public TaskDto GetTaskWithCoursesById(int id, int userId)
        {
            var taskDto = GetTaskById(id, userId);
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(id);
            return taskDto;
        }

        public TaskDto GetTaskWithAnswersById(int id, int userId)
        {
            var taskDto = GetTaskById(id, userId);
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetStudentAnswersToTaskByTaskId(id);
            return taskDto;
        }

        public List<TaskDto> GetTasks()
        {
            var tasks = _taskRepository.GetTasks();
            return tasks;
        }

        public TaskDto AddTask(TaskDto taskDto)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            if (taskDto.Tags == null || taskDto.Tags.Count == 0)
                return _taskRepository.GetTaskById(taskId);

            taskDto.Tags.ForEach(tag => AddTagToTask(taskId, tag.Id));
            return _taskRepository.GetTaskById(taskId);
        }

        public TaskDto UpdateTask(TaskDto taskDto, int taskId)
        {
            _taskValidationHelper.CheckTaskExistence(taskDto.Id);
            taskDto.Id = taskId;
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
        public List<GroupDto> GetGroupsByTaskId(int taskId) => _taskRepository.GetGroupsByTaskId(taskId);
    }
}