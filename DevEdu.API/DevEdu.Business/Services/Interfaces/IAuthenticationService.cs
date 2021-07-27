using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface IAuthenticationService
    {
        string SignIn(UserDto dto);
        string HashPassword(string pass, byte[] salt = default);
    }
}