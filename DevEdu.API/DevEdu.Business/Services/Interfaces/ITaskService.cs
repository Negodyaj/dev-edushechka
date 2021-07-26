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
        public TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds);
        public TaskDto AddTaskByTeacher(TaskDto taskDto, List<int> groupsIds, List<int> tagsIds);
        public TaskDto UpdateTask(TaskDto taskDto, int taskId, int userId);
        public void DeleteTask(int taskId, int userId);
        public int AddTagToTask(int taskId, int tagId);
        public void DeleteTagFromTask(int taskId, int tagId);
        public List<GroupTaskDto> GetGroupTasksByTaskId(int taskId);
    }
}