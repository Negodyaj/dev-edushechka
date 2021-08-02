using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Repositories;

namespace DevEdu.Business.ValidationHelpers
{
    public class MaterialValidationHelper : IMaterialValidationHelper
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialValidationHelper(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public void CheckMaterialExistence(int materialId)
        {
            var material = _materialRepository.GetMaterialById(materialId);
            if (material == default)
                throw new EntityNotFoundException(string.Format(ServiceMessages.EntityWithIdNotFoundMessage, nameof(material), materialId));
        }
    }
}