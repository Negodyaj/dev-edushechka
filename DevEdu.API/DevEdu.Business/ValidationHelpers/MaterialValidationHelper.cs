using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class MaterialValidationHelper : IMaterialValidationHelper
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly ICourseRepository _courseRepository;

        public MaterialValidationHelper(
            IMaterialRepository materialRepository,
            IGroupRepository groupRepository,
            ICourseRepository courseRepository)
        {
            _materialRepository = materialRepository;
            _groupRepository = groupRepository;
            _courseRepository = courseRepository;
        }

        public MaterialDto GetMaterialByIdAndThrowIfNotFound(int materialId)
        {
            var material = _materialRepository.GetMaterialById(materialId);
            if (material == default)
                throw new EntityNotFoundException(string.
                    Format(ServiceMessages.EntityNotFoundMessage, nameof(material), materialId));
            return material;
        }

        public void CheckMethodistAccessToMaterialForDeleteAndUpdate(int userId, MaterialDto material)
        {
            if (material.Courses == null || material.Courses.Count == 0)
            {
                throw new AuthorizationException(string.
                    Format(ServiceMessages.AccessToMaterialDenied, userId, material.Id));
            }
        }

        public void CheckTeacherAccessToMaterialForDeleteAndUpdate(int userId, MaterialDto material)
        {
            if (material.Groups == null || 
                material.Groups.Count == 0 || 
                GetMaterialAllowedToUserByGroup(material.Id, userId) == null) 
            {
                throw new AuthorizationException(string.
                    Format(ServiceMessages.AccessToMaterialDenied, userId, material.Id));
            }
        }

        public void CheckUserAccessToMaterialForGetById(int userId, MaterialDto material)
        {
            if (GetMaterialAllowedToUserByGroup(material.Id, userId) == null &&
                 GetMaterialAllowedToUserByCourse(material.Id, userId) == null)
            {
                throw new AuthorizationException(string.
                    Format(ServiceMessages.AccessToMaterialDenied, userId, material.Id));
            }
        }
        public List<MaterialDto> GetMaterialsAllowedToUser(List<MaterialDto> materials, int userId)
        {
            var materialDtos = new List<MaterialDto>();
            materials.ForEach(m => materialDtos.Add(GetMaterialAllowedToUserByGroup(m.Id, userId)));
            materials.ForEach(m => materialDtos.Add(GetMaterialAllowedToUserByCourse(m.Id, userId)));
            var result = materialDtos.Where(x => x != null).GroupBy(m => m.Id).Select(m => m.First()).ToList();
            return result;
        }

        public void CheckPassedValuesAreUnique(List<int> values, string entity)
        {
            if (!(values.Distinct().Count() == values.Count()))
                throw new ValidationException(string.Format(ServiceMessages.DuplicateValuesProvided, entity));
        }
        private MaterialDto GetMaterialAllowedToUserByGroup(int materialId, int userId)
        {
            var groupsByMaterial = _groupRepository.GetGroupsByMaterialId(materialId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByMaterial.FirstOrDefault(gm => groupsByUser.Any(gu => gu.Id == gm.Id));
            if (result == default)
                return null;
            return _materialRepository.GetMaterialById(materialId);
        }


        private MaterialDto GetMaterialAllowedToUserByCourse(int materialId, int userId)
        {
            var coursesByMaterial = _courseRepository.GetCoursesByMaterialId(materialId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);
            List<int> coursesByUser = new List<int>();
            groupsByUser.ForEach(group => coursesByUser.Add(group.Course.Id));

            var result = coursesByMaterial.FirstOrDefault(сm => coursesByUser.Any(сu => сu == сm.Id));
            if (result == default)
                return null;
            return _materialRepository.GetMaterialById(materialId);
        }
    }
}