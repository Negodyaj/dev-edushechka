using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using DevEdu.Business.ValidationHelpers;
using DevEdu.Business.IdentityInfo;
using DevEdu.DAL.Enums;

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
        private readonly IUserValidationHelper _userValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository,
            ICourseRepository courseRepository,
            IGroupRepository groupRepository,
            IGroupValidationHelper groupValidationHelper,
            ITagValidationHelper tagValidationHelper,
            ICourseValidationHelper courseValidationHelper,
            IMaterialValidationHelper materilaValidationHelper,
            IUserValidationHelper useraValidationHelper)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _groupValidationHelper = groupValidationHelper;
            _tagValidationHelper = tagValidationHelper;
            _courseValidationHelper = courseValidationHelper;
            _materilaValidationHelper = materilaValidationHelper;
            _userValidationHelper = useraValidationHelper;
        }

        public List<MaterialDto> GetAllMaterials(UserIdentityInfo user)
        {
            var allMaterials = _materialRepository.GetAllMaterials();
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                return _materilaValidationHelper.GetMaterialsAllowedToUser(allMaterials, user.UserId);
            }
            return allMaterials;
        }

        public MaterialDto GetMaterialByIdWithCoursesAndGroups(int id)
        {
            var dto = _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(id);
            dto.Courses = _courseRepository.GetCoursesByMaterialId(id);
            dto.Groups = _groupRepository.GetGroupsByMaterialId(id);
            return dto;
        }

        public MaterialDto GetMaterialByIdWithTags(int id, UserIdentityInfo user)
        {
            var dto = _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(id);
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                _materilaValidationHelper.CheckUserAccessToMaterialForGetById(user.UserId, dto);
            }
            return dto;
        }

        public int AddMaterialWithGroups(MaterialDto dto, List<int> tags, List<int> groups, UserIdentityInfo user)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(groups, nameof(groups));
            groups.ForEach(group =>
            {
                _groupValidationHelper.CheckGroupExistence(group);
                var currentRole = user.IsTeacher() ? Role.Teacher : Role.Tutor;
                _userValidationHelper.CheckAuthorizationUserToGroup(group, user.UserId, currentRole);
            });
            var materialId = AddMaterial(dto, tags);
            groups.ForEach(group => _groupRepository.AddGroupMaterialReference(group, materialId));
            return materialId;
        }

        public int AddMaterialWithCourses(MaterialDto dto, List<int> tags, List<int> courses)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(courses, nameof(courses));
            courses.ForEach(course => _courseValidationHelper.GetCourseByIdAndThrowIfNotFound(course));

            var materialId = AddMaterial(dto, tags);
            courses.ForEach(course => _courseRepository.AddCourseMaterialReference(course, materialId));
            return materialId;
        }

        public MaterialDto UpdateMaterial(int id, MaterialDto dto, UserIdentityInfo user)
        {
            var material = GetMaterialByIdWithCoursesAndGroups(id);
            CheckAccessToMaterialByRole(material, user);

            dto.Id = id;
            _materialRepository.UpdateMaterial(dto);
            return _materialRepository.GetMaterialById(dto.Id);
        }

        public void DeleteMaterial(int id, bool isDeleted, UserIdentityInfo user)
        {
            var material = GetMaterialByIdWithCoursesAndGroups(id);
            CheckAccessToMaterialByRole(material, user);
            _materialRepository.DeleteMaterial(id, isDeleted);
        }

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

        public List<MaterialDto> GetMaterialsByTagId(int tagId, UserIdentityInfo user)
        {
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tagId);

            var allMaterialsByTag = _materialRepository.GetMaterialsByTagId(tagId);
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                return _materilaValidationHelper.GetMaterialsAllowedToUser(allMaterialsByTag, user.UserId);
            }
            return allMaterialsByTag;
        }

        private int AddMaterial(MaterialDto dto, List<int> tags)
        {
            if (tags == null || tags.Count == 0)
                return _materialRepository.AddMaterial(dto);

            _materilaValidationHelper.CheckPassedValuesAreUnique(tags, nameof(tags));
            tags.ForEach(tag => _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tag));

            var materialId = _materialRepository.AddMaterial(dto);
            tags.ForEach(tag => _materialRepository.AddTagToMaterial(materialId, tag));
            return materialId;
        }

        private void CheckAccessToMaterialByRole(MaterialDto material, UserIdentityInfo user)
        {
            if (!user.IsAdmin())
            {
                if (user.IsMethodist())
                {
                    _materilaValidationHelper.CheckMethodistAccessToMaterialForDeleteAndUpdate(user.UserId, material);
                }
                else
                {
                    _materilaValidationHelper.CheckTeacherAccessToMaterialForDeleteAndUpdate(user.UserId, material);
                }
            }
        }

        private void CheckMaterialAndTagExistence(int materialId, int tagId)
        {
            _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFound(materialId);
            _tagValidationHelper.GetTagByIdAndThrowIfNotFound(tagId);
        }
    }
}