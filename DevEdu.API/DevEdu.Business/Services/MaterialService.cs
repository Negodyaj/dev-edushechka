using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using System.Linq;

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
        private readonly IMaterialValidationHelper _materilaValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository, 
            ICourseRepository courseRepository, 
            IGroupRepository groupRepository,
            IGroupValidationHelper groupValidationHelper,
            ITagValidationHelper tagValidationHelper,
            ICourseValidationHelper courseValidationHelper,
            IMaterialValidationHelper materilaValidationHelper)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _groupValidationHelper = groupValidationHelper;
            _tagValidationHelper = tagValidationHelper;
            _courseValidationHelper = courseValidationHelper;
            _materilaValidationHelper = materilaValidationHelper;
        }

        public List<MaterialDto> GetAllMaterials() => _materialRepository.GetAllMaterials();

        public MaterialDto GetMaterialByIdWithCoursesAndGroups(int id)
        {
            var dto = _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(id);
            dto.Courses = _courseRepository.GetCoursesByMaterialId(id);
            dto.Groups = _groupRepository.GetGroupsByMaterialId(id);
            return dto;
        }

        public MaterialDto GetMaterialByIdWithTags(int id)
        {
            //проверять доступ пользователя к материалу (сделать хранимку достпных материалов по userID)
            var dto = _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(id);
            return dto;
        }

        public int AddMaterialWithGroups(MaterialDto dto, List<int> tags, List<int> groups)
        {
            if (!(groups.Distinct().Count() == groups.Count()))
                throw new ValidationException(ServiceMessages.DuplicateGroupsValuesProvided);
            groups.ForEach(group => _groupValidationHelper.CheckGroupExistence(group));

            var materialId = AddMaterial(dto, tags);
            groups.ForEach(group => _groupRepository.AddGroupMaterialReference(group, materialId));
            return materialId;
        }

        public int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses)
        {
            if (!(courses.Distinct().Count() == courses.Count()))
                throw new ValidationException(ServiceMessages.DuplicateCoursesValuesProvided);
            courses.ForEach(course => _courseValidationHelper.CheckCourseExistence(course));

            var materialId = AddMaterial(dto, tags);

            courses.ForEach(course => _courseRepository.AddCourseMaterialReference(course, materialId));
            return materialId;
        }

        public MaterialDto UpdateMaterial(int id, MaterialDto dto, int userId, List<Role> roles)
        {
            var material = GetMaterialByIdWithCoursesAndGroups(id);
            _materilaValidationHelper.CheckUserAccessToMaterialForDeleteAndUpdate(userId, roles, material);
            dto.Id = id;
            _materialRepository.UpdateMaterial(dto);
            return _materialRepository.GetMaterialById(dto.Id);
        }

        public void DeleteMaterial(int id, bool isDeleted, int userId, List<Role> roles)
        {
            var material = _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(id);
            _materilaValidationHelper.CheckUserAccessToMaterialForDeleteAndUpdate(userId, roles, material);
            _materialRepository.DeleteMaterial(id, isDeleted);
        }

        public void AddTagToMaterial(int materialId, int tagId) =>
            _materialRepository.AddTagToMaterial(materialId, tagId);

        public void DeleteTagFromMaterial(int materialId, int tagId) =>
            _materialRepository.DeleteTagFromMaterial(materialId, tagId);

        public List<MaterialDto> GetMaterialsByTagId(int tagId)
        {
            _tagValidationHelper.CheckTagExistence(tagId);
            var allMaterialsByTag = _materialRepository.GetMaterialsByTagId(tagId);
            var availableMaterials = new List<MaterialDto>();
            return _materialRepository.GetMaterialsByTagId(tagId);
            //проверка какие материалы дотупны конкретному пользователю и возвращать их,через хранимку новую
        }

        private int AddMaterial(MaterialDto dto, List<int> tags)
        {
            if (tags == null || tags.Count == 0)
            {
                return _materialRepository.AddMaterial(dto);
            }

            if (!(tags.Distinct().Count() == tags.Count()))
                throw new ValidationException(ServiceMessages.DuplicateTagsValuesProvided);

            tags.ForEach(tag => _tagValidationHelper.CheckTagExistence(tag));
            var materialId = _materialRepository.AddMaterial(dto);
            tags.ForEach(tag => AddTagToMaterial(materialId, tag));
            return materialId;
        }
    }
}