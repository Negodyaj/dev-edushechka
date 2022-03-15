using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IMaterialRepository
    {
        Task<int> AddMaterialAsync(MaterialDto material, int courseId);
        Task<int> DeleteOrRestoreMaterialAsync(int id, bool isDeleted);
        Task<MaterialDtoWithCourseId> GetMaterialByIdAsync(int id);
        Task<int> UpdateMaterialAsync(MaterialDto material);
        Task<List<MaterialDto>> GetMaterialsByCourseIdAsync(int courseId);
    }
}