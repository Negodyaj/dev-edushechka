using DevEdu.DAL.Models;
using System.Collections.Generic;

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
        public TaskDto UpdateTask(TaskDto taskDto);
        void DeleteTask(int id);
        public int AddTagToTask(int taskId, int tagId);
        public void DeleteTagFromTask(int taskId, int tagId);
    }
}