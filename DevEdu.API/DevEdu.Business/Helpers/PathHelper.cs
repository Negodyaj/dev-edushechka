using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Text;

namespace DevEdu.Business.Helpers
{
    public static class PathHelper
    {
        private const string _folderUserPhotoPath = "/media/userPhoto/";

        public static string GetPathToSavePhoto(IFileHelper fileHelper, IFormFile photo)
        {
            var sbPathToSavePhoto = new StringBuilder();
            sbPathToSavePhoto.Append(_folderUserPhotoPath);
            sbPathToSavePhoto.Append(fileHelper.ComputeFileHash(photo));
            sbPathToSavePhoto.Append(DateTime.Now.ToString("yyyyMMddhhmmss"));
            sbPathToSavePhoto.Append(Path.GetExtension(photo.FileName));
            return sbPathToSavePhoto.ToString();
        }
    }
}
