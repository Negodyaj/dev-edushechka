using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using System.Threading.Tasks;
using DevEdu.Business.ValidationHelpers;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly ICourseValidationHelper _courseValidationHelper;
        private readonly IMaterialRepository _materialRepository;
        private readonly IMaterialValidationHelper _materialValidationHelper;

        public MaterialService(
            IMaterialRepository materialRepository,
            IMaterialValidationHelper materialValidationHelper,
            ICourseValidationHelper courseValidationHelper)
        {
            _materialRepository = materialRepository;
            _materialValidationHelper = materialValidationHelper;
            _courseValidationHelper = courseValidationHelper;
        }

        public async Task UpdateMaterialAsync(int id, MaterialDto dto)
        {
            await _materialValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(id);
            dto.Id = id;
            await _materialRepository.UpdateMaterialAsync(dto);
        }

        public async Task DeleteMaterialAsync(int id)
        {
            await _materialValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(id);
            await _materialRepository.DeleteOrRestoreMaterialAsync(id, true);
        }

        public async Task RestoreMaterialAsync(int id)
        {
            await _materialValidationHelper.GetMaterialByIdAndThrowIfNotFoundAsync(id);
            await _materialRepository.DeleteOrRestoreMaterialAsync(id, false);
        }

        public async Task<int> AddMaterialAsync(MaterialDto dto, int courseId)
        {
            await _courseValidationHelper.GetCourseByIdAndThrowIfNotFoundAsync(courseId);

            return await _materialRepository.AddMaterialAsync(dto, courseId);
        }
    }
}