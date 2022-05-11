using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;

namespace DevEdu.Business.ValidationHelpers
{
    public class MaterialValidationHelper : IMaterialValidationHelper
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialValidationHelper(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<MaterialDtoWithCourseId> GetMaterialByIdAndThrowIfNotFoundAsync(int materialId)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(materialId);
            if (material == default)
                throw new EntityNotFoundException(string.
                    Format(ServiceMessages.EntityNotFoundMessage, nameof(material), materialId));

            return material;
        }
    }
}