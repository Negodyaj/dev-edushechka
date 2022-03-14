﻿using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class MaterialValidationHelper : IMaterialValidationHelper
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IMaterialRepository _materialRepository;

        public MaterialValidationHelper(
            IMaterialRepository materialRepository,
            IGroupRepository groupRepository,
            ICourseRepository courseRepository)
        {
            _materialRepository = materialRepository;
            _groupRepository = groupRepository;
            _courseRepository = courseRepository;
        }

        public async Task<MaterialDto> GetMaterialByIdAndThrowIfNotFoundAsync(int materialId)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(materialId);
            if (material == default)
                throw new EntityNotFoundException(string.
                    Format(ServiceMessages.EntityNotFoundMessage, nameof(material), materialId));

            return material;
        }

        public void CheckMethodistAccessToMaterialForDeleteAndUpdate(int userId, MaterialDto material)
        {
            if (material.Courses == null ||
                material.Courses.Count == 0)
            {
                throw new AuthorizationException(string.
                    Format(ServiceMessages.AccessToMaterialDenied, userId, material.Id));
            }
        }

        public void CheckUserAccessToMaterialForGetById(int userId, MaterialDto material)
        {
            if (GetMaterialIfAllowedToUserByCourseAsync(material, userId).Result == null)
            {
                throw new AuthorizationException(string.
                    Format(ServiceMessages.AccessToMaterialDenied, userId, material.Id));
            }
        }

        public List<MaterialDto> GetMaterialsAllowedToUser(List<MaterialDto> materials, int userId)
        {
            var materialDtos = new List<MaterialDto>();
            materials.ForEach(async m => materialDtos.Add(await GetMaterialIfAllowedToUserByCourseAsync(m, userId)));
            var result = materialDtos.Where(m => m != null).GroupBy(m => m.Id).Select(m => m.First()).ToList();

            return result;
        }

        public void CheckPassedValuesAreUnique(List<int> values, string entity)
        {
            if (values.Distinct().Count() != values.Count)
                throw new ValidationException(entity, string.Format(ServiceMessages.DuplicateValuesProvided, entity));
        }

        private async Task<MaterialDto> GetMaterialIfAllowedToUserByCourseAsync(MaterialDto material, int userId)
        {
            var coursesByMaterial = await _courseRepository.GetCoursesByMaterialIdAsync(material.Id);
            var groupsByUser = await _groupRepository.GetGroupsByUserIdAsync(userId);

            var result = coursesByMaterial.FirstOrDefault(сm => groupsByUser.Select(t=> t.Course.Id).Any(сu => сu == сm.Id));
            if (result == default)
                material = default;
            
            return material;
        }
    }
}