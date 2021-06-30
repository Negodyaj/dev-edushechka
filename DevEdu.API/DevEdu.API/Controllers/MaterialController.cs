using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevEdu.API.Controllers
{
    public class MaterialController : Controller
    {
        public MaterialController() { }

        [HttpPost]
        public int AddMaterial([FromBody] MaterialInputModel material)
        {
            return 42;
        }
    }
}
