using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.DAL.Repositories
{
    public interface IMaterialRepository
    {
        int AddMaterial(MaterialDto material);
        void AddTagToMaterial(int materialId, int tagId);
        int DeleteMaterial(int id, bool isDeleted);
        int DeleteTagFromMaterial(int materialId, int tagId);
        List<MaterialDto> GetAllMaterials();
        MaterialDto GetMaterialById(int id);
        int UpdateMaterial(MaterialDto material);
        List<MaterialDto> GetMaterialsByTagId(int tagId);
    }
}