using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IMaterialService
    {
        List<MaterialDto> GetAllMaterials(UserIdentityInfo user);
        Task<MaterialDto> GetMaterialByIdWithCoursesAndGroupsAsync(int id);
        MaterialDto GetMaterialByIdWithTags(int id, UserIdentityInfo user);
        int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses);
        int AddMaterialWithGroups(MaterialDto dto, List<int> tags, List<int> groups, UserIdentityInfo user);
        Task<MaterialDto> UpdateMaterialAsync(int id, MaterialDto dto, UserIdentityInfo user);
        Task DeleteMaterialAsync(int id, bool isDeleted, UserIdentityInfo user);
        void AddTagToMaterial(int materialId, int tagId);
        void DeleteTagFromMaterial(int materialId, int tagId);
        List<MaterialDto> GetMaterialsByTagId(int tagId, UserIdentityInfo user);
    }
}