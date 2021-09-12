using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        public TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds, UserIdentityInfo userIdentityInfo);
        public Task<TaskDto> AddTaskByTeacher(TaskDto taskDto, HomeworkDto homework, int groupId, List<int> tagsIds, UserIdentityInfo userIdentityInfo);

        public TaskDto UpdateTask(TaskDto taskDto, int taskId, UserIdentityInfo userIdentityInfo);
        public int DeleteTask(int taskId, UserIdentityInfo userIdentityInfo);
        public TaskDto GetTaskById(int taskId, UserIdentityInfo userIdentityInfo);
        TaskDto GetTaskWithCoursesById(int taskId, UserIdentityInfo userIdentityInfo);
        public TaskDto GetTaskWithAnswersById(int taskId, UserIdentityInfo userIdentityInfo);

        public TaskDto GetTaskWithGroupsById(int taskId, UserIdentityInfo userIdentityInfo);
        public List<TaskDto> GetTasks(UserIdentityInfo userIdentityInfo);
        public int AddTagToTask(int taskId, int tagId, UserIdentityInfo userIdentityInfo);
        public int DeleteTagFromTask(int taskId, int tagId);
    }
}