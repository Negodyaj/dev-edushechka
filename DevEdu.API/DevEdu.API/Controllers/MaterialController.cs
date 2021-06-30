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

        [HttpPost]
        public int AddMaterial([FromBody] MaterialInputModel material)
        {
            return 42;
        }

        [HttpPost("{materialId}/tag/{tagId}")]
        public int AddTagMaterial(int tagId, int materialId)
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
