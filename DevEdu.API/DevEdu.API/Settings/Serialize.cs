using Newtonsoft.Json;

namespace DevEdu.API.Settings
{
    public class Serialize
    {
        public string Serializer()
        {
            var model = new SettingsModel();
            return JsonConvert.SerializeObject
            (
                model,
                Formatting.Indented,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }
            );
        }
    }
}