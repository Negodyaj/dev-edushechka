using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ITaskRepository
    {
        int AddTask(TaskDto task);
        void UpdateTask(TaskDto task);
        int DeleteTask(int id);
        TaskDto GetTaskById(int id);
        List<TaskDto> GetTasks();
        int AddTagToTask(int taskId, int tagId);
        int DeleteTagFromTask(int taskId, int tagId);
        public List<TaskDto> GetTasksByCourseId(int courseId);
    }
}