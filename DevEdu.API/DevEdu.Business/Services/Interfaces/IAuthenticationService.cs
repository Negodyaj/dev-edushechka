using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IAuthenticationService
    {
        Task<string> SignIn(UserDto dto);
        Task<byte[]> GetSalt();
        Task<string> HashPassword(string pass, byte[] salt = null);
        Task<bool> Verify(string hashedPassword, string userPassword);
    }
}