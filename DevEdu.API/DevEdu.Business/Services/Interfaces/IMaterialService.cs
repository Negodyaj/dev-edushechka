using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IMaterialService
    {
        List<MaterialDto> GetAllMaterials();
        public MaterialDto GetMaterialByIdWithCoursesAndGroups(int id);
        public MaterialDto GetMaterialByIdWithTags(int id);
        public int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses);
        public int AddMaterialWithGroups(MaterialDto dto, List<int> tags, List<int> groups);
        MaterialDto UpdateMaterial(int id, MaterialDto dto, int userId, List<Role> roles);
        public void DeleteMaterial(int id, bool isDeleted, int userId, List<Role> roles);
        void AddTagToMaterial(int materialId, int tagId);
        void DeleteTagFromMaterial(int materialId, int tagId);
        List<MaterialDto> GetMaterialsByTagId(int tagId);
    }
}