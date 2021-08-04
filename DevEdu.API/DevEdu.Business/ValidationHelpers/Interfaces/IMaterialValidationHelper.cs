using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IMaterialValidationHelper
    {
        MaterialDto GetMaterialByIdAndThrowIfNotFound(int materialId);
        void CheckMethodistAccessToMaterialForDeleteAndUpdate(int userId, MaterialDto material);
        void CheckTeacherAccessToMaterialForDeleteAndUpdate(int userId, MaterialDto material);
        void CheckUserAccessToMaterialForGetById(int userId, MaterialDto material);
        List<MaterialDto> GetMaterialsAllowedToUser(List<MaterialDto> materials, int userId);
        void CheckPassedValuesAreUnique(List<int> values, string entity);
    }
}