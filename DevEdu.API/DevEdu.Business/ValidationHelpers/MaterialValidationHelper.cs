using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DevEdu.Business.ValidationHelpers
{
    public class MaterialValidationHelper : IMaterialValidationHelper
    {
        private readonly IMaterialRepository _materialRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGroupRepository _groupRepository;

        public MaterialValidationHelper(
            IMaterialRepository materialRepository,
            IUserRepository userRepository,
            IGroupRepository groupRepository)
        {
            _materialRepository = materialRepository;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }

        public MaterialDto GetMaterialByIdAndThrowIfNotFound(int materialId)
        {
            var material = _materialRepository.GetMaterialById(materialId);
            if (material == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityNotFoundMessage, nameof(material), materialId));
            return material;
        }

        public void CheckUserAccessToMaterialForDeleteAndUpdate(int userId, List<Role> roles, MaterialDto material)
        {
            foreach(int role in roles)
            {
                if (role == (int)Role.Methodist)
                {
                    if (material.Courses == null || material.Courses.Count == 0)
                    {
                        throw new AuthorizationException(string.Format(ServiceMessages.AccessToMaterialDenied, userId, material.Id));
                    }
                }
                else if (role == (int)Role.Teacher)
                {
                    if (material.Groups == null || material.Groups.Count == 0)
                    {
                        throw new AuthorizationException(string.Format(ServiceMessages.AccessToMaterialDenied, userId, material.Id));
                    }
                    //check that teacher has access to groups (Отдельный метод)
                }
            }
        }

        public void CheckUserAccessToMaterialForGetById(int userId, List<Role> roles, MaterialDto material)
        {
            foreach (int role in roles)
            {
                if (role == (int)Role.Methodist)
                {
                    return;
                }
                else if (role == (int)Role.Teacher)
                {
                    //check that teacher has access to materials by groups
                }
            }
        }

        //метод возвращающий материалы доступные юзеру по группам
        public MaterialDto GetMaterialAllowedToUser(int materialId, int userId)
        {
            var groupsByMaterial = _groupRepository.GetGroupsByMaterialId(materialId);
            var groupsByUser = _groupRepository.GetGroupsByUserId(userId);

            var result = groupsByMaterial.FirstOrDefault(gm => groupsByUser.Any(gu => gu.Id == gm.Id));
            if (result == default)
                return null;
            return _materialRepository.GetMaterialById(materialId);
            //добавить курсыяы
        }

        public List<TaskDto> GetTasksAllowedToUser(List<TaskDto> tasks, int userId)
        {
            var taskDtos = new List<TaskDto>();
            foreach (var task in tasks)
            {
                taskDtos.Add(GetMaterialAllowedToUser(task.Id, userId));
            }
            return taskDtos;
        }
    }
}