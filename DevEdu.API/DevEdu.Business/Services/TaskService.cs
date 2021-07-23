using System.Collections.Generic;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;

        public TaskService(
            ITaskRepository taskRepository,
            ICourseRepository courseRepository,
            IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository)
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
        }

        public TaskDto GetTaskById(int id)
        {
            // check if task exists
            var taskDto = _taskRepository.GetTaskById(id);
            if (taskDto == default)
                throw new EntityNotFoundException($"task with id = {id} was not found");
            return taskDto;
        }

        public TaskDto GetTaskWithCoursesById(int id)
        {
            var taskDto = _taskRepository.GetTaskById(id);
            if (taskDto == default)
                throw new EntityNotFoundException($"task with id = {id} was not found");
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(id);
            return taskDto;
        }

        public TaskDto GetTaskWithAnswersById(int id)
        {
            var taskDto = _taskRepository.GetTaskById(id);
             if (taskDto == default)
                throw new EntityNotFoundException($"task with id = {id} was not found");
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetStudentAnswersToTaskByTaskId(id);
            return taskDto;
        }

        public TaskDto GetTaskWithCoursesAndAnswersById(int id)
        {
            var taskDto = _taskRepository.GetTaskById(id);
            if (taskDto == default)
                throw new EntityNotFoundException($"task with id = {id} was not found");
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(id);
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetStudentAnswersToTaskByTaskId(id);
            return taskDto;
        }

        public List<TaskDto> GetTasks()
        {
            var tasks = _taskRepository.GetTasks();
            if (tasks == default)
                throw new EntityNotFoundException($"not found any task");
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
            var task = _taskRepository.GetTaskById(taskDto.Id);
            if (task == default)
                throw new EntityNotFoundException($"task with id = {taskDto.Id} was not found");
            _taskRepository.UpdateTask(taskDto);
            return _taskRepository.GetTaskById(taskDto.Id);
        }

        public void DeleteTask(int id)
        {
            var task = _taskRepository.GetTaskById(id);
            if (task == default)
                throw new EntityNotFoundException($"task with id = {id} was not found");
            _taskRepository.DeleteTask(id);
        }

        public int AddTagToTask(int taskId, int tagId) => _taskRepository.AddTagToTask(taskId, tagId);

        public void DeleteTagFromTask(int taskId, int tagId) => _taskRepository.DeleteTagFromTask(taskId, tagId);
    }
}