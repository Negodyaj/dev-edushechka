using System;
using System.Security.Cryptography;

namespace DevEdu.Business.Services
{
    public class AuthenticationService : IAuthenticationService
    {
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
    }
}