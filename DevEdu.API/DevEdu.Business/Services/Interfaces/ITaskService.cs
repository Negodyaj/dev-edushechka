using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        public TaskDto GetTaskById(int id, int userId);
        TaskDto GetTaskWithCoursesById(int id, int userId);
        public TaskDto GetTaskWithAnswersById(int id, int userId);
        List<TaskDto> GetTasks();
        public TaskDto AddTask(TaskDto taskDto);
        public TaskDto UpdateTask(TaskDto taskDto, int taskId);
        void DeleteTask(int id);
        public int AddTagToTask(int taskId, int tagId);
        public void DeleteTagFromTask(int taskId, int tagId);
        public List<GroupDto> GetGroupsByTaskId(int taskId);
    }
}