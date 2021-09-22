using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IHomeworkRepository
    {
        Task<int> AddHomeworkAsync(HomeworkDto homeworkDto);
        Task DeleteHomeworkAsync(int Id);
        Task UpdateHomeworkAsync(HomeworkDto homeworkDto);
        Task<HomeworkDto> GetHomeworkAsync(int id);
        Task<List<HomeworkDto>> GetHomeworkByGroupIdAsync(int groupId);
        Task<List<HomeworkDto>> GetHomeworkByTaskIdAsync(int taskId);
    }
}