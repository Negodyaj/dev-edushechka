using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;

        public MaterialService(IMaterialRepository materialRepository, 
            ITagRepository tagRepository, ICourseRepository courseRepository, 
            IGroupRepository groupRepository)
        {
            _materialRepository = materialRepository;
            _tagRepository = tagRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
        }

        public List<MaterialDto> GetAllMaterials() => _materialRepository.GetAllMaterials();

        public MaterialDto GetMaterialById(int id) 
        {
            var dto = _materialRepository.GetMaterialById(id);
            dto.Courses = _courseRepository.GetCoursesByMaterialId(id);
            dto.Groups = _groupRepository.GetGroupsByMaterialId(id);
            return dto;
        }

        public int AddMaterial(MaterialDto dto)
        {
            var materialId = _materialRepository.AddMaterial(dto);
            if (dto.Tags == null || dto.Tags.Count == 0)
                return materialId;

            dto.Tags.ForEach(tag => AddTagToMaterial(materialId, tag.Id));
            return materialId;
        }

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

        public List<MaterialDto> GetMaterialsByTagId(int tagId)
        {
            var materialDto = _materialRepository.GetMaterialsByTagId(tagId);
            foreach (MaterialDto material in materialDto)
            {
                material.Tags = _tagRepository.GetTagsByMaterialId(material.Id);
            }
            return materialDto;
        }
    }
}