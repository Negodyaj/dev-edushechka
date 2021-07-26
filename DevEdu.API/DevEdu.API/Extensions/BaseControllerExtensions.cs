using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Extensions
{
    public static class BaseControllerExtensions
    {
        public static int GetUserId(this ControllerBase controller)
        {
            var userId = Convert.ToInt32(controller.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            return userId;
        }
    }
}
