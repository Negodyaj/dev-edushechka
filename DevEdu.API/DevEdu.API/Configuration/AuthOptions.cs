using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.API.Configuration
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена, например auth.myserver.com
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена my.server.com
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации подписи
        public const int LIFETIME = 5; // время жизни токена - 5 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}