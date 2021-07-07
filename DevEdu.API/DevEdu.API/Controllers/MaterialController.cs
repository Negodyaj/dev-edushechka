using DevEdu.API.Models.InputModels;
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
        private readonly ITagRepository _tagRepository;
        public MaterialController(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        // api/material
        [HttpPost]
        public int AddMaterial([FromBody] MaterialInputModel materialModel)
        {
            return 42;
        }

        // api/material
        [HttpGet]
        public string GetAllMaterials()
        {
            return "all materials";
        }

        // api/material/5
        [HttpGet("{id}")]
        public string GetMaterial(int id)
        {
            return $"material with id {id}";
        }

        // api/material
        [HttpPut]
        public string UpdateMaterial(int id, [FromBody] MaterialInputModel materialModel)
        {
            return $"updated material with id {id}";
        }

        // api/material/5
        [HttpDelete("{id}")]
        public string DeleteMaterial(int id)
        {
            return $"deleted material with id {id}";
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpPost("{materialId}/tag/{tagId}")]
        public int AddTagToMaterial(int materialId, int tagId)
        {
            _tagRepository.AddTagToMaterial(materialId, tagId);
            return 1;
        }

        // api/material/{materialId}/tag/{tagId}
        [HttpDelete("{materialId}/tag/{tagId}")]
        public string DeleteTagFromMaterial(int materialId, int tagId)
        {
            _tagRepository.DeleteTagFromMaterial(materialId, tagId);
            return $"deleted tag material with {materialId} materialId";
        }

    }
}
