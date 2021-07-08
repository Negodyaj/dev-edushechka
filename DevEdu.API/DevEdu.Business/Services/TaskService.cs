using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
