using System.IO;

namespace DevEdu.API.Settings
{
    public class LoadSettings
    {
        public SettingsModel ReadSettings(string path)
        {
            SettingsModel model;
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    var deserializer = new Deserialize();
                    model = deserializer.Deserializer(streamReader.ReadToEnd());
                }
            }
            return model;
        }
    }
}