using DevEdu.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController
    {
        private readonly AuthenticationService _service;
        public AuthenticationController()
        {
            _service = new AuthenticationService();
        }

        [HttpGet]
        [Description("Sign in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public void SignIn()
        {
            var hashedPass = _service.HashPassword("wertyuhai");
            _service.Verify(hashedPass, "wertyuhao");
        }
    }
}