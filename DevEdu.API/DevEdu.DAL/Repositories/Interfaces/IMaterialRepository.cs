using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IMaterialRepository
    {
        int AddMaterial(MaterialDto material);
        void AddTagToMaterial(int materialId, int tagId);
        void DeleteMaterial(int id);
        void DeleteTagFromMaterial(int materialId, int tagId);
        List<MaterialDto> GetAllMaterials();
        MaterialDto GetMaterialById(int id);
        void UpdateMaterial(MaterialDto material);
    }
}