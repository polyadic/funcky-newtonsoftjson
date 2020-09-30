using Newtonsoft.Json;

namespace Funcky.NewtonsoftJson
{
    public static class JsonSerializerSettingsExtensions
    {
        public static JsonSerializerSettings AddFunckyConverters(this JsonSerializerSettings settings) => settings.AddOptionConverter();

        public static JsonSerializerSettings AddOptionConverter(this JsonSerializerSettings settings)
        {
            settings.Converters.Add(new JsonOptionConverter());
            return settings;
        }
    }
}
