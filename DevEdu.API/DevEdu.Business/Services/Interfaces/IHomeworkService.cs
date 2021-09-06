using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IHomeworkService
    {
        HomeworkDto GetHomework(int homeworkId, UserIdentityInfo userInfo);
        List<HomeworkDto> GetHomeworkByGroupId(int groupId, UserIdentityInfo userInfo);
        List<HomeworkDto> GetHomeworkByTaskId(int taskId);
        HomeworkDto AddHomework(int groupId, int taskId, HomeworkDto dto, UserIdentityInfo userInfo);
        void DeleteHomework(int homeworkId, UserIdentityInfo userInfo);
        HomeworkDto UpdateHomework(int homeworkId, HomeworkDto dto, UserIdentityInfo userInfo);
    }
}