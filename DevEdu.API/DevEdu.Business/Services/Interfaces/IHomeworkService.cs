using System.Collections.Generic;
using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface IHomeworkService
    {
        HomeworkDto AddHomework(int groupId, int taskId, HomeworkDto dto);
        void DeleteHomework(int homeworkId);
        HomeworkDto GetHomework(int homeworkId);
        List<HomeworkDto> GetHomeworkByGroupId(int groupId);
        List<HomeworkDto> GetHomeworkByTaskId(int taskId);
        HomeworkDto UpdateHomework(int homeworkId, HomeworkDto dto);
    }
}