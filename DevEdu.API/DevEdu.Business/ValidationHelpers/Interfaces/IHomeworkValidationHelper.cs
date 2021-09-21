using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IHomeworkValidationHelper
    {
        Task<HomeworkDto> GetHomeworkByIdAndThrowIfNotFoundAsync(int homeworkId);
    }
}