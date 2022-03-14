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
        private readonly ICourseValidationHelper _courseValidationHelper;
        private readonly IMaterialValidationHelper _materilaValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository,
            ICourseRepository courseRepository,
            ICourseValidationHelper courseValidationHelper,
            IMaterialValidationHelper materilaValidationHelper)
        {
            _materialRepository = materialRepository;
            _courseRepository = courseRepository;
            _courseValidationHelper = courseValidationHelper;
            _materilaValidationHelper = materilaValidationHelper;
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

        public async Task<MaterialDto> GetMaterialByIdWithCoursesAsync(int id)
        {
            var dto = await _materilaValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(id);
            dto.Courses = await _courseRepository.GetCoursesByMaterialIdAsync(id);

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

        public async Task<int> AddMaterialWithCoursesAsync(MaterialDto dto, List<int> courses)
        {
            _materilaValidationHelper.CheckPassedValuesAreUnique(courses, nameof(courses));
            foreach (var course in courses)
            {
                await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(course);
            }

            var materialId = await AddMaterialAsync(dto);
            courses.ForEach(async course => await _courseRepository.AddCourseMaterialReferenceAsync(course, materialId));
            return materialId;
        }

        public async Task<MaterialDto> UpdateMaterialAsync(int id, MaterialDto dto, UserIdentityInfo user)
        {
            var material = await GetMaterialByIdWithCoursesAsync(id);
            CheckAccessToMaterialByRole(material, user);

            dto.Id = id;
            await _materialRepository.UpdateMaterialAsync(dto);
            return await _materialRepository.GetMaterialByIdAsync(dto.Id);
        }

        public async Task DeleteMaterialAsync(int id, bool isDeleted, UserIdentityInfo user)
        {
            var material = await GetMaterialByIdWithCoursesAsync(id);
            CheckAccessToMaterialByRole(material, user);
            await _materialRepository.DeleteMaterialAsync(id, isDeleted);
        }

        public async Task<int> AddMaterialAsync(MaterialDto dto)
        {
            return await _materialRepository.AddMaterialAsync(dto);
        }

        private void CheckAccessToMaterialByRole(MaterialDto material, UserIdentityInfo user)
        {
            if (!user.IsAdmin() && user.IsMethodist())
                _materilaValidationHelper.CheckMethodistAccessToMaterialForDeleteAndUpdate(user.UserId, material);

        }
    }
}