using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Servicies
{
    public interface ITaskService
    {
        TaskDto GetTaskById(int id);
        List<TaskDto> GetTasks();
        int AddTask(TaskDto taskDto);
        void UpdateTask(int id, TaskDto taskDto);
        void DeleteTask(int id);
    }
}