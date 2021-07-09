using DevEdu.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }
        public void AddTagToMaterial(int materialId, int tagId)
        {
            _materialRepository.AddTagToMaterial(materialId, tagId);
        }
        public void DeleteTagFromMaterial(int materialId, int tagId)
        {
            _materialRepository.DeleteTagFromMaterial(materialId, tagId);
        }

    }
}
