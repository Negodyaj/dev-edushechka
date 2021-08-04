using System.Collections.Generic;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        public TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds);
        public TaskDto AddTaskByTeacher(TaskDto taskDto, GroupTaskDto groupTask, int groupId, List<int> tagsIds);
        public TaskDto UpdateTask(TaskDto taskDto, int taskId, UserIdentityInfo userIdentityInfo);
        public int DeleteTask(int taskId, UserIdentityInfo userIdentityInfo);
        public TaskDto GetTaskById(int taskid, UserIdentityInfo userIdentityInfo);
        TaskDto GetTaskWithCoursesById(int taskid, UserIdentityInfo userIdentityInfo);
        public TaskDto GetTaskWithAnswersById(int taskid, UserIdentityInfo userIdentityInfo);
        public TaskDto GetTaskWithGroupsById(int taskid, UserIdentityInfo userIdentityInfo);
        public List<TaskDto> GetTasks(UserIdentityInfo userIdentityInfo);
        public int AddTagToTask(int taskId, int tagId);
        public int DeleteTagFromTask(int taskId, int tagId);
    }
}