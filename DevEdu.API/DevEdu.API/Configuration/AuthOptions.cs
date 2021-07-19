using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DevEdu.API.Configuration
{
    public class AuthOptions
    {
        public const string _issuer = "MyAuthServer"; // for example auth.myserver.com
        public const string _audience = "MyAuthClient"; // потребитель токена myserver.com
        const string _key = "mysupersecret_secretkey!123";   // ключ для шифрации подписи
        public const int _lifetime = 5; // время жизни токена - 5 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}