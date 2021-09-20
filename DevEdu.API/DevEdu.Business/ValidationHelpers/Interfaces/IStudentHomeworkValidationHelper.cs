using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentHomeworkValidationHelper
    {
        Task<StudentHomeworkDto> GetStudentHomeworkByIdAndThrowIfNotFoundAsync(int id);
        Task CheckUserInStudentHomeworkAccessAsync(int studentId, int userId);
        Task CheckUserBelongsToHomeworkAsync(int groupId, int userId);
        void CheckUserComplianceToStudentHomework(int studentId, int userId);
    }
}