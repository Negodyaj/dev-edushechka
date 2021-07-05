using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : Controller
    {
        public MaterialController() { }

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
