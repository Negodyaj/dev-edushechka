using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IHomeworkRepository
    {
        int AddHomework(HomeworkDto homeworkDto);
        void DeleteHomework(int Id);
        void UpdateHomework(HomeworkDto homeworkDto);
        HomeworkDto GetHomework(int id);
        List<HomeworkDto> GetHomeworkByGroupId(int groupId);
        List<HomeworkDto> GetHomeworkByTaskId(int taskId);
    }
}