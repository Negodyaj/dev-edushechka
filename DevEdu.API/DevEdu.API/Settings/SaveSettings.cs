using Newtonsoft.Json;
using System.IO;

namespace DevEdu.API.Settings
{
    public class SaveSettings
    {
        public void WriteSettings(string path)
        {
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    var serializer = new Serialize();
                    streamWriter.WriteLine(serializer.Serializer());
                }
            }
        }
    }
}