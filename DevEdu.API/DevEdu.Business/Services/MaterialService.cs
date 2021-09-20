using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<List<MaterialDto>> GetAllMaterialsAsync(UserIdentityInfo user)
        {
            var allMaterials = await _materialRepository.GetAllMaterialsAsync();
            if (!(user.IsAdmin() ||
                user.IsMethodist()))
            {
                return _materilaValidationHelper.GetMaterialsAllowedToUser(allMaterials, user.UserId);
            }

            return allMaterials;
        }

        public async Task<MaterialDto> GetMaterialByIdWithCoursesAndGroupsAsync(int id)
        {
            var dto = await _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(id);
            dto.Courses = await _courseRepository.GetCoursesByMaterialIdAsync(id);
            dto.Groups = await _groupRepository.GetGroupsByMaterialIdAsync(id);

            return dto;
        }

        public async Task<MaterialDto> GetMaterialByIdWithTagsAsync(int id, UserIdentityInfo user)
        {
            var dto = await _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(id);
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                _materilaValidationHelper.CheckUserAccessToMaterialForGetById(user.UserId, dto);
            }

            return dto;
        }

        public async Task<int> AddMaterialWithGroupsAsync(MaterialDto dto, List<int> tags, List<int> groups, UserIdentityInfo user)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(groups, nameof(groups));
            groups.ForEach(async group =>
            {
                var groupDto = await _groupValidationHelper.CheckGroupExistenceAsync(group);
                if (user.IsAdmin())
                    return;

                var currentRole = user.IsTeacher() ? Role.Teacher : Role.Tutor;
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(group, user.UserId, currentRole);
            });
            var materialId = await AddMaterialAsync(dto, tags);
            groups.ForEach(group => _groupRepository.AddGroupMaterialReferenceAsync(group, materialId));
            return materialId;
        }

        public async Task<int> AddMaterialWithCoursesAsync(MaterialDto dto, List<int> tags, List<int> courses)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(courses, nameof(courses));
            courses.ForEach(course => _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(course));

            var materialId = await AddMaterialAsync(dto, tags);
            courses.ForEach(course => _courseRepository.AddCourseMaterialReferenceAsync(course, materialId));
            return materialId;
        }

        public async Task<MaterialDto> UpdateMaterialAsync(int id, MaterialDto dto, UserIdentityInfo user)
        {
            var material = await GetMaterialByIdWithCoursesAndGroupsAsync(id);
            CheckAccessToMaterialByRole(material, user);

            dto.Id = id;
            await _materialRepository.UpdateMaterialAsync(dto);
            return await _materialRepository.GetMaterialByIdAsync(dto.Id);
        }

        public async Task DeleteMaterialAsync(int id, bool isDeleted, UserIdentityInfo user)
        {
            var material = await GetMaterialByIdWithCoursesAndGroupsAsync(id);
            CheckAccessToMaterialByRole(material, user);
            await _materialRepository.DeleteMaterialAsync(id, isDeleted);
        }

        public async Task AddTagToMaterialAsync(int materialId, int tagId)
        {
            await CheckMaterialAndTagExistenceAsync(materialId, tagId);
            await _materialRepository.AddTagToMaterialAsync(materialId, tagId);
        }

        public async Task DeleteTagFromMaterialAsync(int materialId, int tagId)
        {
            await CheckMaterialAndTagExistenceAsync(materialId, tagId);
            await _materialRepository.DeleteTagFromMaterialAsync(materialId, tagId);
        }

        public async Task<List<MaterialDto>> GetMaterialsByTagIdAsync(int tagId, UserIdentityInfo user)
        {
            await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(tagId);

            var allMaterialsByTag = await _materialRepository.GetMaterialsByTagIdAsync(tagId);
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                return _materilaValidationHelper.GetMaterialsAllowedToUser(allMaterialsByTag, user.UserId);
            }

            return allMaterialsByTag;
        }

        private async Task<int> AddMaterialAsync(MaterialDto dto, List<int> tags)
        {
            if (tags == null || tags.Count == 0)
                return await _materialRepository.AddMaterialAsync(dto);

            _materilaValidationHelper.CheckPassedValuesAreUnique(tags, nameof(tags));
            tags.ForEach(async tag => await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(tag));

            var materialId = await _materialRepository.AddMaterialAsync(dto);
            tags.ForEach(async tag => await _materialRepository.AddTagToMaterialAsync(materialId, tag));

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

        private async Task CheckMaterialAndTagExistenceAsync(int materialId, int tagId)
        {
            await _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(materialId);
            await _tagValidationHelper.GetTagByIdAndThrowIfNotFoundAsync(tagId);
        }
    }
}