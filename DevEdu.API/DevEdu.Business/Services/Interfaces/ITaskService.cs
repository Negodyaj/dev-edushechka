using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        public TaskDto GetTaskByIdWithValidation(int id, int userId);
        TaskDto GetTaskById(int id);
        TaskDto GetTaskWithCoursesById(int id);
        public TaskDto GetTaskWithAnswersById(int id);
        List<TaskDto> GetTasks();
        int AddTask(TaskDto taskDto);
        public TaskDto UpdateTask(TaskDto taskDto);
        void DeleteTask(int id);
        public int AddTagToTask(int taskId, int tagId);
        public void DeleteTagFromTask(int taskId, int tagId);
        List<GroupTaskDto> GetGroupsByTaskId(int taskId);
    }
}