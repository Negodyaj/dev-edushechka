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
        private readonly IGroupRepository _groupRepository;
        private readonly ITaskValidationHelper _taskValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public TaskService(
            ITaskRepository taskRepository,
            ICourseRepository courseRepository,
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository,
            IGroupRepository groupRepository,
            ITaskValidationHelper taskValidationHelper,
            IUserValidationHelper userValidationHelper
        )
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
            _groupRepository = groupRepository;
            _taskValidationHelper = taskValidationHelper;
            _userValidationHelper = userValidationHelper;
        }

        public TaskDto GetTaskById(int id, int userId)
        {
            _userValidationHelper.CheckUserExistence(userId);
            var taskDto = _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(id);
            _taskValidationHelper.CheckUserAccessToTask(id, userId);
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

        public TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds )
        {
            var taskId = _taskRepository.AddTask(taskDto);
            var task = _taskRepository.GetTaskById(taskId);
            if (tagsIds != null || tagsIds.Count != 0)
                tagsIds.ForEach(tagId => AddTagToTask(taskId, tagId));
            if (coursesIds != null || coursesIds.Count != 0)
                coursesIds.ForEach(courseId => _courseRepository.AddTaskToCourse(courseId, taskId));

            return task;
        }

        public TaskDto AddTaskByTeacher(TaskDto taskDto, GroupTaskDto groupTask, int groupId, List<int> tagsIds)
        {
            var taskId = _taskRepository.AddTask(taskDto);
            var task = _taskRepository.GetTaskById(taskId);
            if (tagsIds != null || tagsIds.Count != 0)
                tagsIds.ForEach(tagId => AddTagToTask(taskId, tagId));
            if (groupTask != null)
            {
                groupTask.Group = _groupRepository.GetGroup(groupId);
                groupTask.Task = task;
                _groupRepository.AddTaskToGroup(groupTask);
            }
            return task;
        }

        public TaskDto UpdateTask(TaskDto taskDto, int taskId, int userId)
        {
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            _taskValidationHelper.CheckUserAccessToTask(taskId, userId);

            taskDto.Id = taskId;
            _taskRepository.UpdateTask(taskDto);
            return _taskRepository.GetTaskById(taskId);
        }

        public void DeleteTask(int taskId, int userId)
        {
            _taskValidationHelper.GetTaskByIdAndThrowIfNotFound(taskId);
            _taskValidationHelper.CheckUserAccessToTask(taskId, userId);

            _taskRepository.DeleteTask(taskId);
        }

        public int AddTagToTask(int taskId, int tagId)
        {
            return _taskRepository.AddTagToTask(taskId, tagId);
        }

        public void DeleteTagFromTask(int taskId, int tagId) => _taskRepository.DeleteTagFromTask(taskId, tagId);
        public List<GroupTaskDto> GetGroupTasksByTaskId(int taskId) => _taskRepository.GetGroupTasksByTaskId(taskId);
    }
}