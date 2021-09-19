using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IStudentHomeworkValidationHelper
    {
        StudentHomeworkDto GetStudentHomeworkByIdAndThrowIfNotFound(int id);
        Task CheckUserInStudentHomeworkAccessAsync(int studentId, int userId);
        Task CheckUserBelongsToHomeworkAsync(int groupId, int userId);
        void CheckUserComplianceToStudentHomework(int studentId, int userId);
    }
}