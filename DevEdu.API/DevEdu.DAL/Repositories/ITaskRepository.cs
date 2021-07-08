using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.DAL.Repositories
{
    public interface ITaskRepository
    {
        TaskDto GetTaskById(int id);
        List<TaskDto> GetTasks();
        int AddTask(TaskDto taskDto);
        void UpdateTask(TaskDto taskDto);
        void DeleteTask(int id);
        int AddTagToTagTask(int taskId, int tagId);
        void DeleteTagFromTask(int taskId, int tagId);
    }
}