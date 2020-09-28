using Newtonsoft.Json;

namespace Funcky.NewtonsoftJson
{
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings AddOptionConverter(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new JsonOptionConverter());
            return settings;
        }
    }
}
