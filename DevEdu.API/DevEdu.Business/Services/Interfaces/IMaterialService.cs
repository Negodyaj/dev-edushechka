using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IMaterialService
    {
        List<MaterialDto> GetAllMaterials();
        MaterialDto GetMaterialById(int id);
        public int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses);
        public int AddMaterialWithGroups(MaterialDto dto, List<int> tags, List<int> groups);
        void UpdateMaterial(int id, MaterialDto dto);
        void DeleteMaterial(int id, bool isDeleted);
        void AddTagToMaterial(int materialId, int tagId);
        void DeleteTagFromMaterial(int materialId, int tagId);
        List<MaterialDto> GetMaterialsByTagId(int tagId);
        public MaterialDto GetMaterialByIdWithCoursesAndGroups(int id);
    }
}