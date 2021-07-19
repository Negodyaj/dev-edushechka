using System;
using System.Collections.Generic;
using DevEdu.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using DevEdu.API.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Web;
using AutoMapper;
using AutoMapper.Internal;
using DevEdu.API.Models.InputModels;
using DevEdu.API.Models.OutputModels;
using DevEdu.DAL.Enums;
using DevEdu.DAL.Models;

namespace DevEdu.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationService _service;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthenticationController(IUserService userService, IMapper mapper)
        {
            _service = new AuthenticationService();
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("/register")]
        public int Register([FromBody] UserInsertInputModel model)
        {
            var dto = _mapper.Map<UserDto>(model);
            dto.Password = _service.HashPassword(model.Password);
            var addeduser = _userService.AddUser(dto);
            return addeduser;
        }

        [HttpPost("/signin/{username}/{password}")]
        public IActionResult SignIn(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return BadRequest();
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            //var response = new
            //{
            //    access_token = encodedJwt,
            //    username = identity.Name
            //};
            var response = new List<string>();
            response.Add("access_token: " + encodedJwt);
            foreach (var claim in identity.Claims)
            {
                response.Add(claim.Type + ": " + claim.Value);
            }

            return Json(response);
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _userService.SelectUserByEmail(username);

            var claims = new List<Claim>();
            if (user != null)
            {
                if (_service.Verify(user.Password, password))
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email));
                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString()));
                    }
                    if(user.Roles.Contains(Role.Admin))
                    claims.Add(new Claim("AdminOfDevEdu", "IAmBoss"));
                };

                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}