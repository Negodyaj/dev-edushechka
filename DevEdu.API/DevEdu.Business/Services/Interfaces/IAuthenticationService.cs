using DevEdu.DAL.Models;
using System.Threading.Tasks;

namespace DevEdu.Business.Services
{
    public interface IAuthenticationService
    {
        Task<string> SignInAsync(UserDto dto);
        Task<byte[]> GetSaltAsync();
        Task<string> HashPasswordAsync(string pass, byte[] salt = null);
        Task<bool> VerifyAsync(string hashedPassword, string userPassword);
    }
}