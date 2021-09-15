﻿using DevEdu.Business.Configuration;
using DevEdu.Business.Constants;
using DevEdu.Business.Exceptions;
using DevEdu.DAL.Models;
using DevEdu.DAL.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace DevEdu.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthOptions _options;

        public AuthenticationService(IUserRepository userRepository, IAuthOptions authOptions)
        {
            _userRepository = userRepository;
            _options = authOptions;
        }

        public string SignIn(UserDto dto)
        {
            var identity = GetIdentity(dto.Email, dto.Password);
            if (identity == null)
                throw new EntityNotFoundException(ServiceMessages.EntityNotFoundMessage);

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                notBefore: DateTime.UtcNow,
                claims: identity.Claims,//Here we are adding claims to JWT
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(_options.Lifetime)),
                signingCredentials: new SigningCredentials(_options.GetSymmetricSecurityKey(),
                    SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public byte[] GetSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            return salt;
        }

        public string HashPassword(string pass, byte[] salt = default)
        {
            salt ??= GetSalt();
            var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, 10000, HashAlgorithmName.SHA384);
            var hash = pbkdf2.GetBytes(20);
            var hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            var hashedPassword = Convert.ToBase64String(hashBytes);
            return hashedPassword;
        }

        public bool Verify(string hashedPassword, string userPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            var result = HashPassword(userPassword, salt);
            return result == hashedPassword;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = _userRepository.GetUserByEmail(username);
            if (user == null)
                throw new AuthorizationException(ServiceMessages.EntityNotFoundMessage);

            var claims = new List<Claim>();
            if (!Verify(user.Password, password)) throw new AuthorizationException(ServiceMessages.WrongPasswordMessage);

            claims.Add(new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()));
            claims.AddRange(user.Roles.Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())));
            return new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }
    }
}