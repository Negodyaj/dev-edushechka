using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IMaterialRepository
    {
        Task<int> AddMaterialAsync(MaterialDto material);
        Task<int> DeleteMaterialAsync(int id, bool isDeleted);
        Task<List<MaterialDto>> GetAllMaterialsAsync();
        Task<MaterialDto> GetMaterialByIdAsync(int id);
        Task<int> UpdateMaterialAsync(MaterialDto material);
        Task<List<MaterialDto>> GetMaterialsByCourseIdAsync(int courseId);
    }
}