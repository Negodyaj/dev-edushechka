using DevEdu.API.Models.InputModels;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TagController : Controller
    {
        public TagController()
        {

        }

        [HttpPost]
        public int AddTag([FromBody] TagInputModel model)
        {
            return 42;
        }
    }
}
