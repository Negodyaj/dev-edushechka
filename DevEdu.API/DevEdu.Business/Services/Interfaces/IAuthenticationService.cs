using DevEdu.DAL.Models;

namespace DevEdu.Business.Services
{
    public interface IAuthenticationService
    {
        string SignIn(UserDto dto);
        byte[] GetSalt();
        string HashPassword(string pass, byte[] salt = null);
        bool Verify(string hashedPassword, string userPassword);
    }
}