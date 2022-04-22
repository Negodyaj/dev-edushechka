using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IMaterialService
    {
        Task<List<MaterialDto>> GetAllMaterialsAsync(UserIdentityInfo user);
        Task<MaterialDto> GetMaterialByIdWithCoursesAndGroupsAsync(int id);
        Task<MaterialDto> GetMaterialByIdAsync(int id, UserIdentityInfo user);
        Task<int> AddMaterialWithCoursesAsync(MaterialDto dto, List<int> courses);
        Task<int> AddMaterialWithGroupsAsync(MaterialDto dto, List<int> groups, UserIdentityInfo user);
        Task<MaterialDto> UpdateMaterialAsync(int id, MaterialDto dto, UserIdentityInfo user);
        Task DeleteMaterialAsync(int id, bool isDeleted, UserIdentityInfo user);
        Task<int> AddMaterialAsync(MaterialDto dto);
    }
}