using DevEdu.API.Mappers;
using DevEdu.API.Models.InputModels;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        private MaterialRepository _repository;
        private ModelToDtoMapper _mapper;

        public MaterialController() 
        {
            _repository = new MaterialRepository();
            _mapper = new ModelToDtoMapper();
        }

        // api/material
        [HttpPost]
        public int AddMaterial([FromBody] MaterialInputModel materialModel)
        {
            return _repository.AddMaterial(_mapper.MapMaterial(materialModel));
        }

        // api/material
        [HttpGet]
        public List<MaterialDto> GetAllMaterials()
        {
            return _repository.GetAllMaterials() ;
        }

        // api/material/5
        [HttpGet("{id}")]
        public MaterialDto GetMaterial(int id)
        {
            return _repository.GetMaterialById(id);
        }

        // api/material
        [HttpPut] 
        public void UpdateMaterial(int id, [FromBody] MaterialInputModel materialModel)  
        {
            _repository.UpdateMaterial(id, _mapper.MapMaterial(materialModel));
        }

        // api/material/5
        [HttpDelete("{id}")]
        public void DeleteMaterial(int id)
        {
            _repository.DeleteMaterial(id);
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpPost("{materialId}/tag/{tagId}")]
        public int AddTagToMaterial(int materialId, int tagId)
        {
            return 1;
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpDelete("{materialId}/tag/{tagId}")]
        public string DeleteTagFromMaterial(int materialId, int tagId)
        {
            return $"deleted tag material with {materialId} materialId";
        }

    }
}
