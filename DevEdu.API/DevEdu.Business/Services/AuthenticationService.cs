using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using DevEdu.Business.Configuration;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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

        public byte[] GetSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return salt;
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

        public bool Verify(string hashedPassword, string userPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            string result = HashPassword(userPassword, salt);
            return result == hashedPassword;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _userRepository.SelectUserByEmail(username);

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
    }
}