using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.Extensions;
using DevEdu.Business.ValidationHelpers;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupValidationHelper _groupValidationHelper;
        private readonly ITagValidationHelper _tagValidationHelper;
        private readonly ICourseValidationHelper _courseValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository, 
            ICourseRepository courseRepository, 
            IGroupRepository groupRepository,
            IGroupValidationHelper groupValidationHelper,
            ITagValidationHelper tagValidationHelper,
            ICourseValidationHelper courseValidationHelper)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _groupValidationHelper = groupValidationHelper;
            _tagValidationHelper = tagValidationHelper;
            _courseValidationHelper = courseValidationHelper;
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
            var materialId = 0;
            foreach (int group in groups)
            {
                //_groupValidationHelper.CheckProvidedGroupsAreUnique(groups);
                if (!groups.CheckListValuesAreUnique())
                    throw new ValidationException(ServiceMessages.DuplicateGroupsValuesProvided);
                _groupValidationHelper.CheckGroupExistence(group);
            }

            if (tags == null || tags.Count == 0)
            {
                materialId = _materialRepository.AddMaterial(dto);
            }
            else
            {
                foreach (int tag in tags)
                {
                    if (!tags.CheckListValuesAreUnique())
                        throw new ValidationException(ServiceMessages.DuplicateTagsValuesProvided);
                    _tagValidationHelper.CheckTagExistence(tag);
                }
                materialId = _materialRepository.AddMaterial(dto);
                tags.ForEach(tag => AddTagToMaterial(materialId, tag));
            }
            groups.ForEach(group => _groupRepository.AddGroupMaterialReference(group, materialId));
            return materialId;
        }

        public int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses)
        {
            var materialId = 0;
            foreach (int course in courses)
            {
                //_courseValidationHelper.CheckProvidedCoursesAreUnique(courses);
                if(!courses.CheckListValuesAreUnique())
                    throw new ValidationException(ServiceMessages.DuplicateCoursesValuesProvided);
                _courseValidationHelper.CheckCourseExistence(course);
            }

            if (tags == null || tags.Count == 0)
            {
                materialId = _materialRepository.AddMaterial(dto);
            }
            else
            {
                foreach (int tag in tags)
                {
                    if (!tags.CheckListValuesAreUnique())
                        throw new ValidationException(ServiceMessages.DuplicateTagsValuesProvided);
                    _tagValidationHelper.CheckTagExistence(tag);
                }
                materialId = _materialRepository.AddMaterial(dto);
                tags.ForEach(tag => AddTagToMaterial(materialId, tag));
            }
            courses.ForEach(course => _courseRepository.AddCourseMaterialReference(course, materialId));
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

        public void AddTagToMaterial(int materialId, int tagId) =>
            _materialRepository.AddTagToMaterial(materialId, tagId);

        public void DeleteTagFromMaterial(int materialId, int tagId) =>
            _materialRepository.DeleteTagFromMaterial(materialId, tagId);

        public List<MaterialDto> GetMaterialsByTagId(int tagId) =>
            _materialRepository.GetMaterialsByTagId(tagId);
    }
}