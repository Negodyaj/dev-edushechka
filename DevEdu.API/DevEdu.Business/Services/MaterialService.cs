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

        public MaterialService(IMaterialRepository materialRepository, ICourseRepository courseRepository, 
            IGroupRepository groupRepository)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
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

        public MaterialDto GetMaterialByIdWithCourses(int id)
        {
            var dto = _materialRepository.GetMaterialById(id);
            dto.Courses = _courseRepository.GetCoursesByMaterialId(id);
            return dto;
        }

        public MaterialDto GetMaterialByIdWithGroups(int id)
        {
            var dto = _materialRepository.GetMaterialById(id);
            dto.Groups = _groupRepository.GetGroupsByMaterialId(id);
            return dto;
        }

        public int AddMaterialWithGroups(MaterialDto dto, List<int> tags, List<int> groups)
        {
            var materialId = _materialRepository.AddMaterial(dto);

            groups.ForEach(group => _groupRepository.AddGroupMaterialReference(group, materialId));

            if (tags == null || tags.Count == 0)
                return materialId;

            tags.ForEach(tag => AddTagToMaterial(materialId, tag));
            return materialId;
        }

        public int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses)
        {
            var materialId = _materialRepository.AddMaterial(dto);

            courses.ForEach(course => _courseRepository.AddCourseMaterialReference(course, materialId));

            if (tags == null || tags.Count == 0)
                return materialId;

            tags.ForEach(tag => AddTagToMaterial(materialId, tag));
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

        public List<MaterialDto> GetMaterialsByTagId(int tagId) =>
            _materialRepository.GetMaterialsByTagId(tagId);
    }
}