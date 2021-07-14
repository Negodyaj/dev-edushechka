using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        TaskDto GetTaskById(int id);
        TaskDto GetTaskWithCoursesById(int id);
        public TaskDto GetTaskWithAnswersById(int id);
        public TaskDto GetTaskWithCoursesAndAnswersById(int id);
        List<TaskDto> GetTasks();
        int AddTask(TaskDto taskDto);
        void UpdateTask(int id, TaskDto taskDto);
        void DeleteTask(int id);
    }
}