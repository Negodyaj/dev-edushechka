using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace DevEdu.API.Extensions
{
    public static class ControllerExtensions
    {
        public static string GetUserId(this Controller i)
        {
            var userId =  i.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
