using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ITaskRepository
    {
        int AddTask(TaskDto task);
        void UpdateTask(TaskDto task);
        void DeleteTask(int id);
        TaskDto GetTaskById(int id);
        List<TaskDto> GetTasks();
        int AddTagToTask(int taskId, int tagId);
        void DeleteTagFromTask(int taskId, int tagId);
        public List<TaskDto> GetTasksByCourseId(int courseId);
    }
}