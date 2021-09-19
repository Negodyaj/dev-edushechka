using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.DAL.Repositories
{
    public interface IMaterialRepository
    {
        Task<int> AddMaterialAsync(MaterialDto material);
        Task AddTagToMaterialAsync(int materialId, int tagId);
        Task<int> DeleteMaterialAsync(int id, bool isDeleted);
        Task<int> DeleteTagFromMaterialAsync(int materialId, int tagId);
        Task<List<MaterialDto>> GetAllMaterialsAsync();
        Task<MaterialDto> GetMaterialByIdAsync(int id);
        Task<int> UpdateMaterialAsync(MaterialDto material);
        Task<List<MaterialDto>> GetMaterialsByTagIdAsync(int tagId);
        Task<List<MaterialDto>> GetMaterialsByCourseIdAsync(int courseId);
    }
}