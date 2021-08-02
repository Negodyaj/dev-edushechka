using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;
using System.Collections.Generic;

namespace DevEdu.Business.ValidationHelpers
{
    public interface IMaterialValidationHelper
    {
        public MaterialDto GetMaterialByIdAndThrowIfNotFound(int materialId);
        public void CheckUserAccessToMaterialForDeleteAndUpdate(int userId, List<Role> roles, MaterialDto material);
    }
}