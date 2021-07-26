using Newtonsoft.Json;

namespace DevEdu.API.Settings
{
    public class Deserialize
    {
        public SettingsModel Deserializer(string data)
        {
            SettingsModel model;
            try
            {
                model = JsonConvert.DeserializeObject<SettingsModel>(data);
            }
            catch
            {
                model = null;
            }
            return model;
        }
    }
}