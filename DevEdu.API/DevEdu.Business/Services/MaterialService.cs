using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public List<MaterialDto> GetAllMaterials() => _materialRepository.GetAllMaterials();

        public MaterialDto GetMaterialById(int id) => _materialRepository.GetMaterialById(id);

        public int AddMaterial(MaterialDto dto) => _materialRepository.AddMaterial(dto);

        public void UpdateMaterial(int id, MaterialDto dto)
        {
            dto.Id = id;
            _materialRepository.UpdateMaterial(dto);
        }

        public void DeleteMaterial(int id, bool isDeleted) => 
            _materialRepository.DeleteMaterial(id, isDeleted);

        public void AddTagToMaterial(int materialId, int tagId) => 
            _materialRepository.AddTagToMaterial(materialId, tagId);

        public void DeleteTagFromMaterial(int materialId, int tagId) => 
            _materialRepository.DeleteTagFromMaterial(materialId, tagId);

        public List<MaterialDto> GetMaterialsByTagId(int tagId) => 
            _materialRepository.GetMaterialsByTagId(tagId);
    }
}
