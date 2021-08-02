using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMaterialValidationHelper _materialValidationHelper;
        private readonly ITagValidationHelper _tagValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository,
            ICourseRepository courseRepository,
            IGroupRepository groupRepository,
            IMaterialValidationHelper materialValidationHelper,
            ITagValidationHelper tagValidationHelper)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _materialValidationHelper = materialValidationHelper;
            _tagValidationHelper = tagValidationHelper;
        }

        public List<MaterialDto> GetAllMaterials() => _materialRepository.GetAllMaterials();

        public MaterialDto GetMaterialById(int id) => _materialRepository.GetMaterialById(id);

        public MaterialDto GetMaterialByIdWithCoursesAndGroups(int id)
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

        public MaterialDto UpdateMaterial(int id, MaterialDto dto)
        {
            dto.Id = id;
            _materialRepository.UpdateMaterial(dto);
            return _materialRepository.GetMaterialById(dto.Id);
        }

        public void DeleteMaterial(int id, bool isDeleted) =>
            _materialRepository.DeleteMaterial(id, isDeleted);

        public void AddTagToMaterial(int materialId, int tagId)
        {
            CheckMaterialAndTagExistence(materialId, tagId);
            _materialRepository.AddTagToMaterial(materialId, tagId);
        }
        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            CheckMaterialAndTagExistence(materialId, tagId);
            _materialRepository.DeleteTagFromMaterial(materialId, tagId);
        }
        public List<MaterialDto> GetMaterialsByTagId(int tagId) =>
            _materialRepository.GetMaterialsByTagId(tagId);
        private void CheckMaterialAndTagExistence(int materialId, int tagId)
        {
            _materialValidationHelper.CheckMaterialExistence(materialId);
            _tagValidationHelper.CheckTagExistence(tagId);
        }
    }
}