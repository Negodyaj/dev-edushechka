using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface ITaskService
    {
        TaskDto AddTaskByMethodist(TaskDto taskDto, List<int> coursesIds, List<int> tagsIds);
        Task<TaskDto> AddTaskByTeacher(TaskDto taskDto, HomeworkDto homework, int groupId, List<int> tagsIds);

        TaskDto UpdateTask(TaskDto taskDto, int taskId, UserIdentityInfo userIdentityInfo);
        int DeleteTask(int taskId, UserIdentityInfo userIdentityInfo);
        TaskDto GetTaskById(int taskId, UserIdentityInfo userIdentityInfo);
        TaskDto GetTaskWithCoursesById(int taskId, UserIdentityInfo userIdentityInfo);
        Task<TaskDto> GetTaskWithAnswersByIdAsync(int taskId, UserIdentityInfo userIdentityInfo);

        TaskDto GetTaskWithGroupsById(int taskId, UserIdentityInfo userIdentityInfo);
        List<TaskDto> GetTasks(UserIdentityInfo userIdentityInfo);
        int AddTagToTask(int taskId, int tagId);
        int DeleteTagFromTask(int taskId, int tagId);
    }
}