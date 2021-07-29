using DevEdu.Business.Configuration;
using DevEdu.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DevEdu.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }

        public string SignIn(UserDto dto)
        {
            var identity = GetIdentity(dto.Email, dto.Password);
            if (identity == null)
            {
                return null;
            }

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,//Here we are adding claims to JWT
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.Lifetime)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
        public string HashPassword(string pass, byte[] salt = default)
        {
            if (salt == default)
            {
                salt = GetSalt();
            }
            var pkbdf2 = new Rfc2898DeriveBytes(pass, salt, 10000, HashAlgorithmName.SHA384);
            byte[] hash = pkbdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            string hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }
        private byte[] GetSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return salt;
        }


        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _userService.SelectUserByEmail(username);

            var claims = new List<Claim>();
            if (user != null)
            {
                if (Verify(user.Password, password))
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
        private bool Verify(string hashedPassword, string userPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            string result = HashPassword(userPassword, salt);
            return result == hashedPassword;
        }

    }
}