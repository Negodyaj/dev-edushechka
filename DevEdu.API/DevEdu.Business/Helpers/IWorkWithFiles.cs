using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DevEdu.Business.Helpers
{
    public interface IWorkWithFiles
    {
        string ComputeFileHash(IFormFile package);
        Task CreateFile(string path, IFormFile file);
        void TryDeleteFile(string path);
    }
}