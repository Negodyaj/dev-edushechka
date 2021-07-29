namespace DevEdu.Business.Services
{
    public interface IAuthenticationService
    {
        byte[] GetSalt();
        string HashPassword(string pass, byte[] salt = null);
        bool Verify(string hashedPassword, string userPassword);
    }
}