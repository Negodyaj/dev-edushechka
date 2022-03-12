using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DevEdu.Business.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string ComputeFileHash(IFormFile package)
        {
            var hash = "";
            using (var md5 = MD5.Create())
            {
                using (var streamReader = new StreamReader(package.OpenReadStream()))
                {
                    hash = BitConverter.ToString(md5.ComputeHash(streamReader.BaseStream)).Replace("-", "");
                }
            }
            return hash;
        }

        public async Task CreateFile(string path, IFormFile file)
        {
            var directoryPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using var fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        public void TryDeleteFile(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    File.Delete(path);
                }
                catch { }
            }
        }
    }
}
