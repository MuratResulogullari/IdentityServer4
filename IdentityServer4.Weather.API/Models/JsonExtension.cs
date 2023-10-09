using Newtonsoft.Json;

namespace IdentityServer4.Weather.API.Models
{
    public static class JsonExtension
    {
        public static T DeserializeJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentNullException(nameof(json));
            }

            T? result = JsonConvert.DeserializeObject<T>(json);

            if (result == null)
            {
                throw new JsonSerializationException($"Failed to deserialize JSON to type {typeof(T).Name}.");
            }

            return result;
        }
    }
}
