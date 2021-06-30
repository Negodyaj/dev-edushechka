using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/material")]
    public class MaterialController : Controller
    {
        public MaterialController() { }

        // api/material
        [HttpPost]
        public int AddMaterial([FromBody] MaterialInputModel materialModel)
        {
            return 42;
        }

        // api/material/all
        [HttpGet("all")]
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

        [HttpPost("{materialId}/tag/{tagId}")]
        public int AddTagMaterial(int materialId, int tagId)
        {
            return 1;
        }

        [HttpDelete("{tagMaterialId}")]
        public string DeleteTagMaterial(int tagMaterialId)
        {
            return $"deleted tag material with {tagMaterialId} Id";
        }

    }
}
