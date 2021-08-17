using DevEdu.Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DevEdu.Business.Configuration
{
    public class AuthOptions : IAuthOptions
    {
        private string _key;   // key for encoding last part of the token

        public string Issuer => "MyAuthServer"; // for example auth.myserver.com
        public string Audience => "MyAuthClient"; // for example myserver.com
        public int Lifetime { get; set; } // 5 minutes

        public AuthOptions(IOptions<AuthSettings> options)
        {
            _key = options.Value.KeyForToken;
            Lifetime = options.Value.TokenLifeTime;
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key));
        }
    }
}