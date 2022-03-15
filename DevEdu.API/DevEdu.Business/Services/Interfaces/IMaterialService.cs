using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IMaterialService
    {
        Task UpdateMaterialAsync(int id, MaterialDto dto);
        Task DeleteMaterialAsync(int id);
        Task RestoreMaterialAsync(int id);
        Task<int> AddMaterialAsync(MaterialDto dto, int courseId);
    }
}