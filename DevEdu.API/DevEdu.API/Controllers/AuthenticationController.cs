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
        public string SignIn(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions._issuer,
                audience: AuthOptions._audience,
                notBefore: now,
                claims: identity.Claims,//Here we are adding claims to JWT
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions._lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _userService.SelectUserByEmail(username);

            var claims = new List<Claim>();
            if (user != null)
            {
                if (_service.Verify(user.Password, password))
                {
                    claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()));
                    foreach (var role in user.Roles)
                    {
                        claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString()));
                    }
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