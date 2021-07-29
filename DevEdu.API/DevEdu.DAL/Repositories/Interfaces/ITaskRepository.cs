using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface ITaskRepository
    {
        TaskDto GetTaskById(int id);
        List<TaskDto> GetTasks();
        int AddTask(TaskDto task);
        void UpdateTask(TaskDto task);
        void DeleteTask(int id);
        int AddTagToTask(int taskId, int tagId);
        void DeleteTagFromTask(int taskId, int tagId);
        List<GroupTaskDto> GetGroupsByTaskId(int groupId);
        List<TaskDto> GetTaskByCourseId(int courseId);
    }
}