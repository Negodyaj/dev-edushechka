using DevEdu.DAL.Models;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IHomeworkValidationHelper
    {
        HomeworkDto GetHomeworkByIdAndThrowIfNotFound(int homeworkId);
    }
}