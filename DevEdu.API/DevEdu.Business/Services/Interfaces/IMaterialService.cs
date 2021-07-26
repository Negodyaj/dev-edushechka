using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public interface IMaterialService
    {
        List<MaterialDto> GetAllMaterials();
        MaterialDto GetMaterialById(int id);
        int AddMaterial(MaterialDto dto, List<int> tags);
        void UpdateMaterial(int id, MaterialDto dto);
        void DeleteMaterial(int id, bool isDeleted);
        void AddTagToMaterial(int materialId, int tagId);
        void DeleteTagFromMaterial(int materialId, int tagId);
        List<MaterialDto> GetMaterialsByTagId(int tagId);
        public MaterialDto GetMaterialByIdWithCoursesAndGroups(int id);
    }
}