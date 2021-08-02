using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IHomeworkService
    {
        HomeworkDto GetHomework(int homeworkId, int userId);
        List<HomeworkDto> GetHomeworkByGroupId(int groupId, int userId);
        List<HomeworkDto> GetHomeworkByTaskId(int taskId);
        HomeworkDto AddHomework(int groupId, int taskId, HomeworkDto dto, int userId);
        void DeleteHomework(int homeworkId, int userId);
        HomeworkDto UpdateHomework(int homeworkId, HomeworkDto dto, int userId);
    }
}