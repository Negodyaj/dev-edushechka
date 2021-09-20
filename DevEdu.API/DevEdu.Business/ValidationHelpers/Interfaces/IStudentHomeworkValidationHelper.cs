using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentHomeworkValidationHelper
    {
        Task CheckUserInStudentHomeworkAccessAsync(int studentId, int userId);
        Task CheckUserBelongsToHomeworkAsync(int groupId, int userId);
        Task<StudentHomeworkDto> GetStudentHomeworkByIdAndThrowIfNotFound(int id);
        void CheckUserComplianceToStudentHomework(int studentId, int userId);
    }
}