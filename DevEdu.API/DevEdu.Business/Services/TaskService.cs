using System.Collections.Generic;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public TaskDto GetTaskById(int id) => _taskRepository.GetTaskById(id);

        public List<TaskDto> GetTasks() => _taskRepository.GetTasks();

        public int AddTask(TaskDto taskDto) => _taskRepository.AddTask(taskDto);

        public void UpdateTask(int id, TaskDto taskDto)
        {
            taskDto.Id = id;
            _taskRepository.UpdateTask(taskDto);
        }

        public void DeleteTask(int id) => _taskRepository.DeleteTask(id);
        public List<GroupTaskDto> GetTaskGroupByTaskId(int taskId) => _taskRepository.GetTaskGroupByTaskId(taskId);
    }
}