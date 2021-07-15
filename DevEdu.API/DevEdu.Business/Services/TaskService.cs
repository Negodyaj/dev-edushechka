using System.Collections.Generic;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentAnswerOnTaskRepository _studentAnswerOnTaskRepository;

        public TaskService(ITaskRepository taskRepository, ICourseRepository courseRepository, IStudentAnswerOnTaskRepository studentAnswerOnTaskRepository)
        {
            _taskRepository = taskRepository;
            _courseRepository = courseRepository;
            _studentAnswerOnTaskRepository = studentAnswerOnTaskRepository;
        }

        public TaskDto GetTaskById(int id) => _taskRepository.GetTaskById(id);

        public TaskDto GetTaskWithCoursesById(int id)
        {
            var taskDto = _taskRepository.GetTaskById(id);
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(id);
            return taskDto;
        }

        public TaskDto GetTaskWithAnswersById(int id)
        {
            var taskDto = _taskRepository.GetTaskById(id);
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetStudentAnswersToTaskByTaskId(id);
            return taskDto;
        }

        public TaskDto GetTaskWithCoursesAndAnswersById(int id)
        {
            var taskDto = _taskRepository.GetTaskById(id);
            taskDto.Courses = _courseRepository.GetCoursesToTaskByTaskId(id);
            taskDto.StudentAnswers = _studentAnswerOnTaskRepository.GetStudentAnswersToTaskByTaskId(id);
            return taskDto;
        }

        public List<TaskDto> GetTasks() => _taskRepository.GetTasks();

        public int AddTask(TaskDto taskDto) => _taskRepository.AddTask(taskDto);

        public void UpdateTask(int id, TaskDto taskDto)
        {
            taskDto.Id = id;
            _taskRepository.UpdateTask(taskDto);
        }

        public void DeleteTask(int id) => _taskRepository.DeleteTask(id);
    }
}