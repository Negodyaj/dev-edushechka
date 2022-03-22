using DevEdu.Business.IdentityInfo;
using DevEdu.Business.ValidationHelpers;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IGroupValidationHelper _groupValidationHelper;
        private readonly ICourseValidationHelper _courseValidationHelper;
        private readonly IMaterialValidationHelper _materilaValidationHelper;
        private readonly IUserValidationHelper _userValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository,
            ICourseRepository courseRepository,
            IGroupRepository groupRepository,
            IGroupValidationHelper groupValidationHelper,
            ICourseValidationHelper courseValidationHelper,
            IMaterialValidationHelper materilaValidationHelper,
            IUserValidationHelper useraValidationHelper)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _groupRepository = groupRepository;
            _groupValidationHelper = groupValidationHelper;
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

        public async Task<MaterialDto> GetMaterialByIdAsync(int id, UserIdentityInfo user)
        {
            var dto = await _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(id);
            if (!(user.IsAdmin() || user.IsMethodist()))
            {
                _materilaValidationHelper.CheckUserAccessToMaterialForGetById(user.UserId, dto);
            }

            return dto;
        }

        public async Task<int> AddMaterialWithGroupsAsync(MaterialDto dto, List<int> groups, UserIdentityInfo user)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(groups, nameof(groups));

            foreach (var group in groups)
            {
                var groupDto = await _groupValidationHelper.CheckGroupExistenceAsync(group);
                if (user.IsAdmin())
                    break;

                var currentRole = user.IsTeacher() ? Role.Teacher : Role.Tutor;
                await _userValidationHelper.CheckAuthorizationUserToGroupAsync(group, user.UserId, currentRole);
            }

            var materialId = await AddMaterialAsync(dto);
            groups.ForEach(group => _groupRepository.AddGroupMaterialReferenceAsync(group, materialId));
            return materialId;
        }

        public async Task<int> AddMaterialWithCoursesAsync(MaterialDto dto, List<int> courses)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(courses, nameof(courses));
            foreach(var course in courses)
            {
                await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(course);
            }

            var materialId = await AddMaterialAsync(dto);
            courses.ForEach(async course => await _courseRepository.AddCourseMaterialReferenceAsync(course, materialId));
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

        public async Task<int> AddMaterialAsync(MaterialDto dto)
        {
            return await _materialRepository.AddMaterialAsync(dto);
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
    }
}