using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IMaterialValidationHelper
    {
        Task<MaterialDtoWithCourseId> GetMaterialByIdAndThrowIfNotFoundAsync(int materialId);
    }
}